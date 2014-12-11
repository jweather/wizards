==Wizards in Printer

Source repository, such as it is.  The main components:

 - MIDIPlayback, a C# Windows app that reads in a MIDI file, allows mapping its channels to different instruments, along with various transformations (such as always taking the lowest note, or transposing).  It writes out the results as `wizards.h`, as well as allowing interactive remapping of channels and various live controls.  Talks to Arduino via serial.
 - sketch_ramps, an Arduino Mega sketch targetted for a RAMPS v1.4 board.  In its default mode, reads a binary protocol from the C# app to trigger individual notes.  Changing an `#if 0` flips it to an ASCII protocol for direct control and testing of individual motors.  Also capable of playing back an embedded sequence from `wizards.h` for more precise timing and headless operation (no PC required).  Also monitors the DISKCHG pin on a floppy drive to detect disk insertion and removal to trigger playback, although this part has always been a bit flaky since I'm not even remotely following the floppy spec.
 
 You want a schematic?  Here's your schematic:
 
 <img src="https://raw.githubusercontent.com/jweather/wizards/master/schematic.jpg">
 