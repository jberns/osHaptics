#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_LSM303_U.h>
#include <Adafruit_L3GD20_U.h>
#include <Adafruit_9DOF.h>

#include <Servo.h> 


/* Assign a unique ID to the sensors */
Adafruit_9DOF                 dof   = Adafruit_9DOF();
Adafruit_LSM303_Accel_Unified accel = Adafruit_LSM303_Accel_Unified(30301);
Adafruit_LSM303_Mag_Unified   mag   = Adafruit_LSM303_Mag_Unified(30302);
Adafruit_L3GD20_Unified       gyro  = Adafruit_L3GD20_Unified(20);

//Variables for IMU
  float mRoll;
  float mPitch;
  float mHeading;
  float gRoll;
  float gPitch;
  float gHeading;  
  float aRoll;
  float aPitch;
  float aHeading;  
  float totalDelay = 1;
  float averageNumber;
  float const pi = 3.14159265359;
  float const radToDeg = 180/pi;
  
//Variables for Servo and Force Sensor
  Servo pointer;
  const int analogInPin = A0;
  float servoMove = 0; 
  float pos = 0;
  float posWrite = 0;
  int sensorValue = 0;        
  int outputValue = 0; 
  int sensorLowerLimit = 0;
  int setPoint = 0;
  int deadZone = 0;
  char incomingCommand;
  
/**************************************************************************/
/*!
    @brief  Initialises all the sensors used by this example
*/
/**************************************************************************/
void initSensors()
{
  if(!accel.begin())
  {
    /* There was a problem detecting the LSM303 ... check your connections */
    Serial.println(F("Ooops, no LSM303 detected ... Check your wiring!"));
    while(1);
  }
  if(!mag.begin())
  {
    /* There was a problem detecting the LSM303 ... check your connections */
    Serial.println("Ooops, no LSM303 detected ... Check your wiring!");
    while(1);
  }
  
  if(!gyro.begin())
  {
    /* There was a problem detecting the L3GD20 ... check your connections */
    Serial.print("Ooops, no L3GD20 detected ... Check your wiring or I2C ADDR!");
    while(1);
  }
  
}

/**************************************************************************/
/*!

*/
/**************************************************************************/
void setup(void)
{
  Serial.begin(115200);
  //Serial.println(F("Adafruit 9 DOF Pitch/Roll/Heading Example")); Serial.println("");
  
  /* Initialise the sensors */
  initSensors();
  
  //Attach servo to pin 9
  pointer.attach(9);
  

}

/**************************************************************************/
/*!
    @brief  Constantly check the roll/pitch/heading/altitude/temperature
*/
/**************************************************************************/
void loop(void)
{
  sensor_t sensor;
  gyro.getSensor(&sensor);
 
  
  sensors_event_t accel_event;
  sensors_event_t mag_event;
  sensors_event_t  gyro_event;
  sensors_vec_t   orientation;

  /* Read the accelerometer and magnetometer */
  accel.getEvent(&accel_event);
  mag.getEvent(&mag_event);
  gyro.getEvent(&gyro_event);

if (dof.fusionGetOrientation(&accel_event, &mag_event, &orientation))
    {
      /* 'orientation' should have valid .roll and .pitch fields */
      aRoll = orientation.roll;
      aPitch = orientation.pitch;
      aHeading = orientation.heading;
         
    }  
 
 
 
if(aHeading <0){
  aHeading = -180-aHeading;
}

if(aHeading >0){
  aHeading = 180-aHeading;
}

 gRoll -= (totalDelay/115)*gyro_event.gyro.y*radToDeg + 0.00*(gRoll + aPitch);
 gPitch += (totalDelay/115)*gyro_event.gyro.x*radToDeg  - 0.00*(gPitch - aRoll);
 gHeading -= (totalDelay/115)*gyro_event.gyro.z*radToDeg + 0.00*(gHeading - aHeading);
 
// Serial.println(aRoll);
// delay(10);

 
 //lastHeading = aHeading;
  
 posWrite = getServoPosition();
 pointer.write(posWrite);
 posWrite = map(posWrite, 15, 180, 0, 80);
 
 incomingCommand = Serial.read();

   if(incomingCommand == 'U'){
      Serial.print(gRoll);
      Serial.print(F(" "));
      Serial.print(gPitch);
      Serial.print(F(" "));
      Serial.print(gHeading);   
      Serial.print(F(" "));
      Serial.print(posWrite);      
      Serial.println(F(""));
   }

    
  delay(totalDelay);
}

float getServoPosition(){
  // read the analog in value:
  sensorValue = analogRead(analogInPin);            
  // map it to the range of the analog out:
  outputValue = map(sensorValue, 0, 140, 0, 100);  

  sensorLowerLimit = 15;
  setPoint = 5;
  deadZone = setPoint + 5;
  
  if (outputValue < setPoint){
    servoMove = 0.3*(sensorLowerLimit - outputValue);
    pos -= servoMove;
  }  
  
  if(outputValue > setPoint + deadZone){
    servoMove = 0.1*(outputValue - setPoint);
    pos += servoMove;
  }
  
  if(pos < sensorLowerLimit){
    pos = sensorLowerLimit;
  }
    
  if (pos > 179){
    pos = 180;
  }
  
  return pos;
  
}
