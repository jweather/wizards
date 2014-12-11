using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sanford.Multimedia.Midi;
using System.Threading;
using System.IO.Ports;

namespace MIDIPlayback {
    public partial class Form1 : Form {
        const int NINST = 9;

        // MIDI file details, should be encapsulated
        int bpm = 120;
        int ppqn = 480;

        Dictionary<byte, Channel> channels = new Dictionary<byte, Channel>();

        OutputDevice midiOut;

        public Form1() {
            InitializeComponent();

            listChannels.FullRowSelect = true;
            listMappings.FullRowSelect = true;
            mapStrat.SelectedIndex = 0;
            comPort.SelectedIndex = 1;

            try {
                midiOut = new OutputDevice(0);
            } catch {
                MessageBox.Show("No local MIDI playback available");
            }

            // wizards
            open("wizards in printer.mid");

            /*
            // postprocess floppy channels with a minimum note duration
            foreach (byte c in new byte[] { 1, 7, 14, 15 }) {
                for (int ni = 0; ni < channels[c].notes.Count; ni++) {
                    TimedNote n = channels[c].notes[ni];
                    if (n.len < 300)
                        n.len = 300;
                }
            }
             */

            // chan->instrument, octave
            quickMap(8, 0, 0);
            quickMap(6, 1, 1, MappingStrategy.Lowest);
            quickMap(12, 2, 0);
            quickMap(3, 3, 0); // printer
            quickMap(15, 4, -1);
            quickMap(14, 5, -1);
            quickMap(7, 6, -1);
            quickMap(1, 7, -1);
            quickMap(10, 8, 1);

            quickMap(0, 7, 0, MappingStrategy.Lowest); // extra bass on bottom floppy

            export("wizards.h");
        }

        private void export(string fname) {
            Console.WriteLine("1 tick = " + tickToMS(1) + " ms");
            uint t = 0;
            List<TimedEvent> seq = buildSequence();
            int count = seq.Count;
            string deltas = "", insts = "", notes = "";
            int ni = 0;
            int skip = 0;
            using (StreamWriter sw = new StreamWriter(fname)) {
                foreach (TimedEvent e in seq) {
                    if (skip > 0) {
                        skip--;
                        t = e.tick;
                        continue;
                    }

                    uint delta = e.tick - t;
                    deltas += delta + ",";
                    insts += e.inst + ",";
                    notes += e.note + ",";
                    t = e.tick;
                    ni++;
                    if (ni == count) break; // stop early
                }
                count++;
                // sleep at the end
                deltas += "20000"; // 8 sec
                insts += "0";
                notes += "0";

                sw.WriteLine("const unsigned int NNOTES = " + count + ";");
                sw.WriteLine("PROGMEM const unsigned int tdata[" + count + "] = {" + deltas + "};");
                sw.WriteLine("PROGMEM const byte idata[" + count + "] = {" + insts + "};");
                sw.WriteLine("PROGMEM const byte ndata[" + count + "] = {" + notes + "};");
            }
        }

        private void quickMap(int chan, int inst, int octave) {
            quickMap(chan, inst, octave, MappingStrategy.Highest);
        }

        private void quickMap(int chan, int inst, int octave, MappingStrategy strategy) {
            Mapping map = new Mapping(channels[(byte)chan]);

            map.instrument = (byte)inst;
            map.octave = octave;
            map.strategy = strategy;

            ListViewItem i = new ListViewItem(new string[] { map.instrument.ToString(), map.chan.chan.ToString(), map.describe() });
            i.Tag = map;
            listMappings.Items.Add(i);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            midiOut.Close();
        }

        private void browse_Click(object sender, EventArgs ea) {
            OpenFileDialog f = new OpenFileDialog();

            f.Filter = "MIDI files|*.mid";
            DialogResult r = f.ShowDialog();
            if (r != System.Windows.Forms.DialogResult.OK)
                return;
            try {
                open(f.FileName);
            } catch (Exception e) {
                MessageBox.Show("Failed to read file: " + e.Message);
            }
        }

        private void open(string fn) {
            int ntracks = 0;
            List<byte[]> sections = new List<byte[]>();
            List<uint> sectionStart = new List<uint>();
            uint offset = 0;
            using (BinaryReader r = new BinaryReader(File.Open(fn, FileMode.Open))) {
                offset = 0;
                string name = new string(r.ReadChars(4)); offset += 4;
                if (name != "MThd") throw new FormatException("not a MIDI file");
                uint sectionLength = read32(r, ref offset);
                ushort ftype = read16(r, ref offset);
                if (ftype != 0x01)
                    throw new FormatException("Only type 1 MIDI files are readable: " + ftype);
                ntracks = read16(r, ref offset);
                ppqn = read16(r, ref offset);
                if ((ppqn & 0x8000) != 0) {
                    ppqn = (ushort)(ppqn & 0x7FFF);
                    throw new FormatException("Can't parse frames per second times: " + ppqn);
                }

                for (int track = 0; track < ntracks; track++) {
                    name = new string(r.ReadChars(4)); offset += 4;
                    sectionLength = read32(r, ref offset);
                    if (name != "MTrk")
                        throw new Exception("unknown non-track section " + name + " @ " + offset);
                    sectionStart.Add(offset);
                    sections.Add(r.ReadBytes((int)sectionLength));
                    offset += sectionLength;
                }
            }

            for (int track = 0; track < ntracks; track++) {
                MemoryStream str = new MemoryStream(sections[track]);
                BinaryReader r = new BinaryReader(str);
                List<byte> trackChannels = new List<byte>();
                offset = sectionStart[track];

                string trkname = "track " + track;
                uint tick = 0;
                byte lastTch = 0;
                try {
                    while (offset - sectionStart[track] < sections[track].Length) {
                        uint delta = readVar(r, ref offset);
                        tick += delta;

                        byte tch = read8(r, ref offset);
                        if (tch == 0xFF) {
                            // meta
                            byte type = read8(r, ref offset);
                            uint metaLen = readVar(r, ref offset);

                            if (type == 0x03) {
                                trkname = new string(r.ReadChars((int)metaLen));
                                offset += metaLen;
                            } else if (type == 0x51) {
                                uint mpqn = read24(r, ref offset);
                                bpm = 60000000 / (int)mpqn;
                                d("mpqn = " + mpqn + ", bpm = " + bpm);
                            } else {
                                r.ReadBytes((int)metaLen);
                                offset += metaLen;
                            }
                        } else if (tch == 0xF0 || tch == 0xF7) {
                            throw new Exception("sysex messages not supported");
                        } else {
                            // normal event
                            byte p1;
                            if ((tch & 0x80) == 0) {
                                // running status (repeat same event type)
                                p1 = tch;
                                tch = lastTch;
                                if (lastTch == 0xFF)
                                    throw new Exception("running status not supported for meta");
                            } else {
                                p1 = read8(r, ref offset);
                                lastTch = tch;
                            }
                            byte type = (byte)(tch >> 4);
                            byte chan = (byte)(tch & 0x0F);

                            byte p2 = 0;
                            if (type != 0xC && type != 0xD)
                                p2 = read8(r, ref offset);

                            uint tactual = tick;
                            // delete a whole section, horrible hack
                            if (tick >= 264960 && tick < 295680)
                                continue;
                            else if (tick >= 295680)
                                tactual -= (295680-264960);

                            if (type == 0x9) {
                                addNote(chan, tactual, p1, p2);
                                if (!trackChannels.Contains(chan)) trackChannels.Add(chan);
                            } else if (type == 0x8) {
                                addNote(chan, tactual, p1, 0);
                            }
                        }
                    }
                } catch (Exception e) {
                    d("Exception " + e + " for track " + track + " offset " + offset);
                }
                /*
                if (trackChannels.Count == 1) {
                    Channel c = channels[trackChannels[0]];
                    c.track = (byte)track;
                    c.name = trkname;
                }*/
            }
            List<Channel> sorted = new List<Channel>(channels.Values);
            sorted.Sort(Channel.byChannel);

            foreach (Channel c in sorted) {
                ListViewItem i = new ListViewItem(new string[] { 
                    c.chan.ToString(), c.name, c.notes.Count.ToString(), c.describe() });
                i.Tag = c;
                listChannels.Items.Add(i);
            }
        }

        private byte read8(BinaryReader r, ref uint read) {
            read++;
            return r.ReadByte();
        }

        private ushort read16(BinaryReader r, ref uint read) {
            ushort val = (ushort)(r.ReadByte() << 8);
            val |= r.ReadByte();
            read += 2;
            return val;
        }

        private uint read24(BinaryReader r, ref uint read) {
            uint val = (uint)(r.ReadByte() << 16);
            val |= (uint)(r.ReadByte() << 8);
            val |= r.ReadByte();
            read += 3;
            return val;
        }

        private uint read32(BinaryReader r, ref uint read) {
            uint val = (ushort)(r.ReadByte() << 24);
            val |= (uint)(r.ReadByte() << 16);
            val |= (uint)(r.ReadByte() << 8);
            val |= r.ReadByte();
            read += 4;
            return val;
        }

        private uint readVar(BinaryReader r, ref uint read) {
            uint val = 0;
            for (int i = 0; i < 4; i++) {
                byte b = r.ReadByte();
                val <<= 7;
                val |= (byte)(b & 0x7F);
                if ((b & 0x80) == 0) {
                    read += (uint)(i + 1);
                    break;
                }
            }
            return val;
        }

        private void addNote(byte chan, uint tick, byte note, byte vel) {
            if (!channels.ContainsKey(chan))
                channels.Add(chan, new Channel(chan));
            channels[chan].addNote(tick, note, vel != 0);
        }

        private void mapAdd_Click(object sender, EventArgs e) {
            Mapping map = buildMapping();
            if (map == null) return;
            ListViewItem i = new ListViewItem(new string[] { map.instrument.ToString(), map.chan.chan.ToString(), map.describe() });
            i.Tag = map;
            listMappings.Items.Add(i);
        }

        private void mapRemove_Click(object sender, EventArgs e) {
            if (listMappings.SelectedItems.Count == 0) return;
            listMappings.Items.Remove(listMappings.SelectedItems[0]);
        }

        private void mapToggle_Click(object sender, EventArgs e) {
            if (listMappings.SelectedItems.Count == 0) return;
            ListViewItem i = listMappings.SelectedItems[0];
            Mapping map = (Mapping)i.Tag;
            map.enabled = !map.enabled;
            i.SubItems[2].Text = map.describe();
        }

        private List<TimedEvent> buildSequence() {
            List<TimedEvent> seq = new List<TimedEvent>();
            foreach (ListViewItem i in listMappings.Items) {
                Mapping m = (Mapping)i.Tag;
                seq.AddRange(m.generate());
            }
            seq.Sort(TimedEvent.byTime);
            return seq;
        }

        bool playing = false;
        private void play_Click(object sender, EventArgs e) {
            if (playing) {
                playing = false;
                midiOutAllOff();
                for (int i = 0; i < NINST; i++) {
                    send(0, i, 0);
                }
                play.Text = "Play";
                return;
            }

            if (listMappings.Items.Count == 0) {
                MessageBox.Show("No mappings added");
                return;
            }

            List<TimedEvent> seq = buildSequence();

            playing = true;
            play.Text = "Stop";

            Thread t = new Thread(new ParameterizedThreadStart(playback));
            t.IsBackground = true;
            t.Start(seq);
        }

        private Mapping buildMapping() {
            if (listChannels.SelectedIndices.Count != 1) {
                MessageBox.Show("Please select a channel to map.");
                return null;
            }

            Mapping map = new Mapping((Channel)listChannels.SelectedItems[0].Tag);

            map.instrument = (byte)mapInst.Value;
            map.octave = (int)mapOctave.Value;
            map.strategy = (MappingStrategy)Enum.ToObject(typeof(MappingStrategy), mapStrat.SelectedIndex);

            return map;
        }

        bool previewPlaying = false;
        private void mapPreviewLocal_Click(object sender, EventArgs e) {
            if (previewPlaying) {
                previewPlaying = false;
                mapPreviewLocal.Text = "Preview";
                midiOutAllOff();
                for (int i = 0; i < NINST; i++) {
                    send(0, i, 0);
                }
                return;
            }

            Mapping map = buildMapping();
            if (map == null) return;

            Thread t = new Thread(new ParameterizedThreadStart(playback));
            t.IsBackground = true;
            previewPlaying = true;
            mapPreviewLocal.Text = "Stop";
            t.Start(map.generate());
        }

        private void playback(object obj) {
            List<TimedEvent> seq = (List<TimedEvent>)obj;
            int skipto = Convert.ToInt16(skipTo.Text);

            DateTime tzero = DateTime.Now - TimeSpan.FromMilliseconds(tickToMS(seq[0].tick));
            double peakLag = 0.0;
            double totalLag = 0.0;
            ChannelMessageBuilder builder = new ChannelMessageBuilder();
            int eventCount = 0;
            foreach (TimedEvent t in seq) {
                if (skipto > 0) {
                    skipto--;
                    if (skipto == 0)
                        tzero = DateTime.Now - TimeSpan.FromMilliseconds(tickToMS(t.tick));
                    else
                        continue;
                }
                DateTime expected = tzero + TimeSpan.FromMilliseconds(tickToMS(t.tick));
                TimeSpan delta = expected - DateTime.Now;
                if (delta.TotalMilliseconds > 0) {
                    d("sleeping " + delta.TotalMilliseconds + " ms until " + t.tick + " (" + tickToMS(t.tick) + " ms) for event " + eventCount);
                    Thread.Sleep(delta);
                }

                double lag = Math.Abs((DateTime.Now - expected).TotalMilliseconds);
                if (lag > peakLag) {
                    d("lag max: " + peakLag + " @" + eventCount);
                    peakLag = lag;
                }
                totalLag += lag;

                if (!previewPlaying && !playing) break;

                d("event " + eventCount + " = " + t.note + " on " + t.inst);
                send(0, t.inst, t.note);
                eventCount++;
            }
            midiOutAllOff();
            // reset button state if we finished naturally
            if (previewPlaying)
                this.BeginInvoke((MethodInvoker)delegate { mapPreviewLocal_Click(null, null); });
            if (playing)
                this.BeginInvoke((MethodInvoker)delegate { play_Click(null, null); });

            d("total lag = " + totalLag + ", average = " + totalLag / (double)seq.Count);
        }
#if false
        InputDevice midiIn = null;
        private void openMIDI_Click(object sender, EventArgs ea) {
            if (midiIn != null) {
                midiIn.Close();
                midiIn = null;
                openMIDI.Text = "Open MIDI";
                return;
            }

            try {
                openMIDI.Text = "Close MIDI";
                midiIn = new InputDevice(0);
                midiIn.ChannelMessageReceived += midiIn_ChannelMessageReceived;
                midiIn.StartRecording();
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                try { midiIn.Close(); } catch { }
                midiIn = null;
                openMIDI.Text = "Open MIDI";
            }
        }

        int lastLiveMIDINote = 0;
        private void midiIn_ChannelMessageReceived(object sender, ChannelMessageEventArgs e) {
            int inst = (int)midiInst.Value;
            if (e.Message.Command == ChannelCommand.NoteOn) {
                lastLiveMIDINote = e.Message.Data1;
                send(0, inst, (byte)e.Message.Data1);
            } else if (e.Message.Command == ChannelCommand.NoteOff) {
                if (e.Message.Data1 == lastLiveMIDINote)
                    send(0, inst, 0);
                lastLiveMIDINote = 0;
            }
        }
#endif

        // send to serial if open, local MIDI if not
        SerialPort serial = null;
        int[] localNote = new int[NINST];
        private void send(int delta, int inst, byte note) {
            if (serial == null) {
                // local MIDI playback
                ChannelMessageBuilder builder = new ChannelMessageBuilder();
                builder.MidiChannel = inst;
                if (note == 0 || note != localNote[inst]) {
                    builder.Command = ChannelCommand.NoteOff;
                    builder.Data1 = localNote[inst];
                    builder.Data2 = 0;
                    builder.Build();
                    midiOut.Send(builder.Result);
                    localNote[inst] = 0;
                }

                if (note != 0) {
                    builder.Command = ChannelCommand.NoteOn;
                    builder.Data1 = note;
                    builder.Data2 = 127;
                    builder.Build();
                    midiOut.Send(builder.Result);
                    localNote[inst] = note;
                }
            } else {
                serial.Write(new byte[] { (byte)(0xF0 + inst), (byte)note }, 0, 2);
            }
        }

        private void comOpen_Click(object sender, EventArgs ea) {
            if (serial != null) {
                serial.Close();
                serial = null;
                comOpen.Text = "Open COM";
                return;
            }
            try {
                serial = new SerialPort(comPort.Text, 57600, Parity.None, 8, StopBits.One);
                serial.Open();
                comOpen.Text = "Close COM";
            } catch (Exception e) {
                MessageBox.Show(e.ToString());
                serial = null;
            }
        }

        public static void d(string msg) {
            Debug.WriteLine(msg);
        }

        private double tickToMS(uint tick) {
            return (double)tick * 60000.0 / (double)ppqn / (double)bpm;
        }

        private void midiOutAllOff() {
            // all notes off -- isn't there an easier way to do this?
            ChannelMessageBuilder builder = new ChannelMessageBuilder();
            builder.Command = ChannelCommand.Controller;
            builder.Data1 = 123;
            builder.Data2 = 0;
            builder.Build();
            midiOut.Send(builder.Result);
        }

        private void allOff_Click(object sender, EventArgs e) {
            playing = false;
            previewPlaying = false;
            midiOutAllOff();
            for (int i = 0; i < NINST; i++) {
                send(0, i, 0);
            }
        }

        const byte ADV_HOME_START = 0x00;
        const byte ADV_HOME_END = 0x01;
        const byte ADV_EMBED_PLAY = 0x02;
        const byte ADV_EMBED_STOP = 0x03;

        private void home_MouseDown(object sender, MouseEventArgs e) {
            int inst = Convert.ToInt32((sender as Button).Tag);
            if (serial != null)
                serial.Write(new byte[] { (byte)(0xE0 + inst), ADV_HOME_START }, 0, 2);
        }

        private void home_MouseUp(object sender, MouseEventArgs e) {
            int inst = Convert.ToInt32((sender as Button).Tag);
            if (serial != null)
                serial.Write(new byte[] { (byte)(0xE0 + inst), ADV_HOME_END }, 0, 2);
        }

        private void notePreview_ValueChanged(object sender, EventArgs e) {
            send(0, (int)mapInst.Value, (byte)notePreview.Value);
        }

        private void noteOff_Click(object sender, EventArgs e) {
            send(0, (int)mapInst.Value, (byte)0);
        }

        private void embedPlay_Click(object sender, EventArgs e) {
            if (serial != null)
                serial.Write(new byte[] { 0xE0, ADV_EMBED_PLAY }, 0, 2);
        }

        private void embedStop_Click(object sender, EventArgs e) {
            if (serial != null)
                serial.Write(new byte[] { 0xE0, ADV_EMBED_STOP }, 0, 2);
            for (int i = 0; i < NINST; i++) {
                send(0, i, 0);
            }
        }
    }

    // MIDI note data
    class Channel {
        public byte chan, track;
        public string name;
        public byte minNote = 255, maxNote = 0;
        public int polyphony = 0;
        public List<TimedNote> notes = new List<TimedNote>();

        Dictionary<byte, uint> noteOn = new Dictionary<byte, uint>();

        public Channel(byte _chan) {
            chan = _chan;
            name = "channel " + chan;
        }

        public Channel(byte _chan, string _name) {
            chan = _chan;
            name = _name;
        }

        public string describe() {
            return minNote + "-" + maxNote + " poly " + polyphony;
        }

        public void addNote(uint tick, byte note, bool on) {
            if (!on) {
                if (!noteOn.ContainsKey(note)) {
                    //Form1.d("note off for ch" + chan + " note " + note + " at " + tick + " with no previous note on");
                    return;
                }

                //Form1.d("ch" + chan + " @" + noteOn[note] + " note " + note + " len " + (tick - noteOn[note]));
                notes.Add(new TimedNote(noteOn[note], tick - noteOn[note], note));
                noteOn.Remove(note);
            } else if (noteOn.ContainsKey(note)) {
                //Form1.d("duplicate note on for ch" + chan + " note " + note + " at " + tick);
            } else {
                //Form1.d("ch" + chan + " @" + tick + " note " + note + " on");
                noteOn.Add(note, tick);
                if (note < minNote) minNote = note;
                if (note > maxNote) maxNote = note;
                if (noteOn.Count > polyphony) polyphony = noteOn.Count;
            }
        }

        static public int byChannel(Channel a, Channel b) {
            if (a.chan > b.chan) return 1;
            if (a.chan < b.chan) return -1;
            return 0;
        }
    }

    // MIDI note
    struct TimedNote {
        public uint tick, len;
        public byte note;
        public TimedNote(uint tick, uint len, byte note) {
            this.tick = tick;
            this.len = len;
            this.note = note;
        }
        static public int byTick(TimedNote a, TimedNote b) {
            if (a.tick > b.tick) return 1;
            if (a.tick < b.tick) return -1;
            return 0;
        }
    }

    enum MappingStrategy { Recent, Highest, Lowest, Arpeggio };

    // MIDI channel to monophonic sequence
    class Mapping {
        public Channel chan;
        public byte instrument;
        public int octave;
        public MappingStrategy strategy;
        public bool enabled;

        public Mapping(Channel chan) {
            this.chan = chan;
            enabled = true;
        }

        public string describe() {
            if (!enabled) return "disabled";
            string desc = strategy.ToString();
            if (octave > 0)
                desc += "+" + octave;
            else if (octave < 0)
                desc += "-" + octave;
            return desc;
        }

        public List<TimedEvent> generate() {
            List<TimedEvent> seq = new List<TimedEvent>();
            if (!enabled) return seq;

            byte currentNote = 0;
            uint currentEnd = 0;
            chan.notes.Sort(TimedNote.byTick);
            foreach (TimedNote n in chan.notes) {
                if (currentNote != 0) {
                    if (currentEnd < n.tick) {
                        // rest
                        seq.Add(new TimedEvent(currentEnd, instrument, 0));
                    } else if (currentEnd > n.tick) {
                        // override?
                        if (strategy == MappingStrategy.Recent ||
                            (strategy == MappingStrategy.Highest && n.note > currentNote) ||
                            (strategy == MappingStrategy.Lowest && n.note < currentNote)) {
                            // allow this note to override previous

                            // same timestamp? remove old note
                            if (n.tick == seq[seq.Count - 1].tick)
                                seq.RemoveAt(seq.Count - 1);
                        } else {
                            // ignore this note and continue the previous one
                            continue;
                        }
                    }
                }

                // staccato repeats
                if (seq.Count > 0 && seq[seq.Count - 1].note == (byte)(n.note + octave * 12)) {
                    seq.Add(new TimedEvent(n.tick-20, instrument, (byte)0));
                }

                seq.Add(new TimedEvent(n.tick, instrument, (byte)(n.note + octave * 12)));
                currentNote = n.note;
                currentEnd = n.tick + n.len;
            }
            seq.Add(new TimedEvent(currentEnd, instrument, 0));

            seq.Sort(TimedEvent.byTime);
            return seq;
        }
    }

    // monophonic note in sequence
    struct TimedEvent {
        public uint tick;
        public byte inst, note;
        public TimedEvent(uint tick, byte inst, byte note) {
            this.tick = tick; this.note = note; this.inst = inst;
        }

        static public int byTime(TimedEvent a, TimedEvent b) {
            if (a.tick > b.tick) return 1;
            if (a.tick < b.tick) return -1;
            return 0;
        }
    }
}

