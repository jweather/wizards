#include "arduino2.h"
#include "TimerOne.h"

// pinout for ATmega1280 with RAMPS 1.4
const int ledPin = 13;
const int readyPin = 2;

// instrument types  
const int T_SCANNER = 0;
const int T_FLOPPY = 1;
const int T_PRINTER_CR = 2; // carriage motor
const int T_PRINTER_PF = 3; // paper feed motor with infinite travel

const int NI = 9;

// X, Y, Z, E0, four floppies, E1
const byte iStep[NI] =            {  54,   60,   46,   26,   16,   25,   31,   37,   36};
const byte iDir[NI] =             {  55,   61,   48,   28,   17,   27,   33,   39,   34};
const byte iLight[NI] =           {   8,    9,   10,    3,   23,   29,   35,   41,    0};
const byte iEnable[NI] =          {  38,   56,   62,   24,    0,    0,    0,    0,   30};
const unsigned int maxSteps[NI] = {4000, 6000, 2000, 6000,   80,   80,   80,   80,    0};
const boolean invertLight[NI] =   {   0,    0,    0,    0,    1,    1,    1,    1,    0};
const byte lightLevel[NI]  =      { 128,    0,    0,   32,    0,    0,    0,    0,    0};
const byte iType[NI] =            {T_SCANNER, T_SCANNER, T_SCANNER, T_PRINTER_CR,
                                    T_FLOPPY, T_FLOPPY, T_FLOPPY, T_FLOPPY, T_PRINTER_PF};
// timers and counters
boolean active[NI], stepped[NI], dir[NI];
unsigned int stepCount[NI];
unsigned int interval[NI], elapsed[NI];

#include "wizards.h"

#define RESOLUTION 40
#define PAUSE 9999

// original usecs between steps
//const unsigned long pitches[256] = {0,115447,108967,102851,97079,91630,86487,81633,77051,72727,68645,64792,61156,57723,54483,51425,48539,45815,43243,40816,38525,36363,34322,32396,30578,28861,27241,25712,24269,22907,21621,20408,19262,18181,17161,16198,15289,14430,13620,12856,12134,11453,10810,10204,9631,9090,8580,8099,7644,7215,6810,6428,6067,5726,5405,5102,4815,4545,4290,4049,3822,3607,3405,3214,3033,2863,2702,2551,2407,2272,2145,2024,1911,1803,1702,1607,1516,1431,1351,1275,1203,1136,1072,1012,955,901,851,803,758,715,675,637,601,568,536,506,477,450,425,401,379,357,337,318,300,284,268,253,238,225,212,200,189,178,168,159,150,142,134,126,119,112,106,100,94,89,84,79,75,71,67,63,59,56,53,50,47,44,42,39,37,35,33,31,29,28,26,25,23,22,21,19,18,17,16,15,14,14,13,12,11,11,10,9,9,8,8,7,7,7,6,6,5,5,5,4,4,4,4,3,3,3,3,3,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

// prescaled for 40usec cycles
const unsigned int pitchScaled[256] = {0,2886,2724,2571,2427,2291,2162,2041,1926,1818,1716,1620,1529,1443,1362,1286,1213,1145,1081,1020,963,909,858,810,764,722,681,643,607,573,541,510,482,455,429,405,382,361,341,321,303,286,270,255,241,227,215,202,191,180,170,161,152,143,135,128,120,114,107,101,96,90,85,80,76,72,68,64,60,57,54,51,48,45,43,40,38,36,34,32,30,28,27,25,24,23,21,20,19,18,17,16,15,14,13,13,12,11,11,10,9,9,8,8,8,7,7,6,6,6,5,5,5,4,4,4,4,4,3,3,3,3,3,3,2,2,2,2,2,2,2,2,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};

int ledState;

void light(int inst, int state) {
  if (iLight[inst] == 0) return;
  state = state ^ invertLight[inst];
  if (lightLevel[inst]) {
    analogWrite(iLight[inst], state ? lightLevel[inst] : 0);
  } else {
    digitalWrite2(iLight[inst], state);
  }
}

void setup() {
  Serial.begin(57600);
  Serial.println("setup");

  pinMode(ledPin, OUTPUT);
  pinMode(readyPin, INPUT_PULLUP);
  
  for (int i=0; i<NI; i++) {
    pinMode(iStep[i], OUTPUT);
    pinMode(iDir[i], OUTPUT);
    if (iEnable[i] != 0) {
      pinMode(iEnable[i], OUTPUT);
      digitalWrite2(iEnable[i], LOW); // enable is inverted
    }
    if (iLight[i] != 0)
      pinMode(iLight[i], OUTPUT);
  }

  // home the floppies
  for (int i=0; i < NI; i++) {
    if (iType[i] == T_FLOPPY) {
      digitalWrite2(iDir[i], HIGH);
      digitalWrite2(iLight[i], LOW); // inverted
    }
  }

  for (int i = 0; i < 80; i++) {
    for (int i=0; i < NI; i++) {
      if (iType[i] == T_FLOPPY)
        digitalWrite2(iStep[i], HIGH);
    }
    delayMicroseconds(1);
    for (int i=0; i < NI; i++) {
      if (iType[i] == T_FLOPPY)
        digitalWrite2(iStep[i], LOW);
    }
    delay(20);
  }
  for (int i=0; i < NI; i++) {
    if (iType[i] == T_FLOPPY)
      digitalWrite2(iDir[i], LOW);
  }
  
  // cycle lights at startup  
  for (int n=0; n<1; n++) {
    for (int i=0; i<NI; i++) {
      light(i, 1);
      delay(100);
      light(i, 0);
    }
  }

  Serial.println("Console");  
  
  Timer1.initialize(RESOLUTION);
  Timer1.attachInterrupt(driverRun);
}

inline void driveMotor(byte i) {
  if (active[i]) {
    if (stepped[i]) {
      digitalWrite2(iStep[i], LOW);
      stepped[i] = 0;
    }      
    elapsed[i]++;
    if (elapsed[i] >= interval[i]) {
      digitalWrite2(iStep[i], HIGH);
      stepped[i] = 1;
      elapsed[i] = 0;
      stepCount[i]++;
      if (stepCount[i] == maxSteps[i]) {
        stepCount[i] = 0;
        dir[i] = !dir[i];
        digitalWrite2(iDir[i], dir[i]);
      }
    }
  }
}

void homeAll() {
  for (int i = 0; i < NI; i++) {
    interval[i] = 0;
    active[i] = 0;
    elapsed[i] = 0;
    light(i, 0);
  }
  
  for (int i = 0; i < NI; i++) {
    if (maxSteps[i] == 0) continue; // doesn't home
    light(i, 1);
    if (dir[i] == 0) { // reverse direction first
      dir[i] = 1;
      digitalWrite2(iDir[i], dir[i]);
    } else {
      // going the right way, just reverse the step count
      stepCount[i] = maxSteps[i] - stepCount[i];
    }
  }
  
  bool done = false;
  while (!done) {
    done = true;
    for (int i = 0; i < NI; i++) {
      if (maxSteps[i] == 0) continue; // doesn't home
      if (stepCount[i] == 0) continue; // homed
      done = false;
      digitalWrite2(iStep[i], HIGH);
      stepCount[i]--;
      if (stepCount[i] == 0) light(i, 0);
    }
    delayMicroseconds(100);
    for (int i = 0; i < NI; i++)
      digitalWrite2(iStep[i], LOW);
      
    delayMicroseconds(pitchScaled[69]*RESOLUTION - 100);
  }
  
  for (int i = 0; i < NI; i++) {
    if (maxSteps[i] == 0) continue;
    light(i, 0);
    dir[i] = 0;
    digitalWrite2(iDir[i], dir[i]);
  }
}

// once every 10*RESOLUTION usecs (0.4msec, approximately = 1 tick for Wizards in Printer)
unsigned int noteIndex = PAUSE;
unsigned int currentDelta = 0;
byte lastReady = 2;
int readySteps = 0;
bool readyReady = 0;
void noteRun() {
  // watch for disk state changes
  byte readyState = digitalRead(readyPin);
  if (readyState != lastReady) {
    if (!readyReady) {
      // ignore
      Serial.print("Ignored ready="); Serial.println(readyState, DEC);
      lastReady = readyState;
      return;
    }
    Serial.println(readyState);
    lastReady = readyState;
    if (readyState == 1) {
      Serial.println("play");
      readySteps = 0;
      digitalWrite(iLight[4], HIGH);
      digitalWrite(iDir[4], LOW);
      noteIndex = 0;
      currentDelta = 5000; // 2sec delay before playback starts
    } else {
      Serial.println("pausing and re-homing all");
      noteIndex = PAUSE; // stop playback and home everything
      readyReady = false;
      homeAll();
    }
  }

  if (noteIndex == PAUSE) {
    // poll for disk while stopped
    readySteps++;
    if (readySteps == 500) {
      digitalWrite(iLight[4], LOW);
      digitalWrite(iDir[4], HIGH);
      digitalWrite(iStep[4], HIGH);
    } else if (readySteps == 501) {
      digitalWrite(iStep[4], LOW);
      readySteps = 0;
      if (!readyReady) {
        Serial.println("ready");
        readyReady = 1;
      }
    }
    
    return; // playback paused
  }
  
  if (currentDelta > 0) {
    currentDelta--;
    if (currentDelta > 0) return; // sleeping
  }

  do {
    byte i = pgm_read_byte_near(idata + noteIndex);
    byte n = pgm_read_byte_near(ndata + noteIndex);
    if (n == 0) {
      interval[i] = 0;
      active[i] = 0;
      elapsed[i] = 0;
      light(i, 0);
    } else {
      interval[i] = pitchScaled[n];
      active[i] = 1;
      light(i, 1);
    }

    noteIndex++;
    if (noteIndex == NNOTES) {
      noteIndex = PAUSE;
      return;
    }
    currentDelta = pgm_read_word_near(tdata + noteIndex);
  } while (currentDelta == 0);
}

byte driverSteps = 0;

// run every 40 usec
void driverRun() {
  driveMotor(0);
  driveMotor(1);
  driveMotor(2);
  driveMotor(3);
  driveMotor(4);
  driveMotor(5);
  driveMotor(6);
  driveMotor(7);
  driveMotor(8);

  driverSteps++;
  if (driverSteps == 10) {
    noteRun();
    driverSteps = 0;
  }
}

int blinkstate = 0;
int octave = 0, lastPitch = 0, inst = 0;
int tuning = 0;

void pitch(int p) {
  interval[inst] = pitchScaled[p + octave*12] + tuning;
  active[inst] = 1;
  light(inst, 1);
  lastPitch = p;
}


void loop() {
#if 1
// computer-controlled input
  if (Serial.available() >= 2) {
    int i = Serial.read();
    int n;
    if (i >= 0xF0 && i < (0xF0+NI)) {
      i -= 0xF0;
      n = Serial.read();
      if (n == 0) {
        interval[i] = 0;
        active[i] = 0;
        elapsed[i] = 0;
        light(i, 0);
      } else {
        interval[i] = pitchScaled[n];
        active[i] = 1;
        light(i, 1);
      }
    } else if (i >= 0xE0 && i < (0xE0+NI)) {
      // extended functions
      i -= 0xE0;
      n = Serial.read();
      switch (n) {
        case 0:
          // start homing
          stepCount[i] = 0;
          dir[i] = 1; digitalWrite2(iDir[i], dir[i]);
          interval[i] = pitchScaled[69]; // Concert A
          active[i] = 1;
          light(i, 1);
          break;
        case 1:
          // stop homing and reset counts
          stepCount[i] = 0;
          dir[i] = 0; digitalWrite2(iDir[i], dir[i]);
          interval[i] = 0;
          elapsed[i] = 0;
          light(i, 0);
          active[i] = 0;
          break;
        case 2:
          // embedded playback
          noteIndex = 0;
          currentDelta = 0;
          break;
        case 3:
          noteIndex = PAUSE;
          break;
      }
    }
    
    blinkstate = !blinkstate;
    digitalWrite2(ledPin, blinkstate);
  }


#else
  //interactive loop
  if (Serial.available() > 0) {
    char c = Serial.read();
    switch (c) {
      case '+':
        octave++;
        Serial.println(octave, DEC);
        pitch(lastPitch);
        break;
      case '-':
        octave--;
        Serial.println(octave, DEC);
        pitch(lastPitch);
        break;
      case '[':
        tuning++;
        Serial.println(tuning, DEC);
        pitch(lastPitch);
        break;
      case ']':
        tuning--;
        Serial.println(tuning, DEC);
        pitch(lastPitch);
        break;
      case 'a': pitch(45); break;
      case 'b': pitch(47); break;
      case 'c': pitch(48); break;
      case 'd': pitch(50); break;
      case 'e': pitch(52); break;
      case 'f': pitch(53); break;
      case 'g': pitch(55); break;
      case '0': inst = 0; break;
      case '1': inst = 1; break;
      case '2': inst = 2; break;
      case '3': inst = 3; break;
      case '4': inst = 4; break;
      case '5': inst = 5; break;
      case '6': inst = 6; break;
      case '7': inst = 7; break;
      case '8': inst = 8; break;
      case 'x':
        for (int i=0; i<NI; i++) {
          interval[i] = 0;
          active[i] = 0;
          light(i, 0);
        }
        break;
      case 'h':
        Serial.println(stepCount[4], DEC);
        break;
      case 'D':
        dir[inst] = !dir[inst];
        digitalWrite2(iDir[inst], dir[inst]);
        Serial.print("dir["); Serial.print(inst, DEC); Serial.print("] = ");
        Serial.println(dir[inst], DEC);
        break;
      case 's':
        digitalWrite2(iStep[inst], HIGH);
        delayMicroseconds(10);
        digitalWrite2(iStep[inst], LOW);
        Serial.print("step["); Serial.print(inst, DEC); Serial.println("]");
        break;
    }
  }
#endif
}


