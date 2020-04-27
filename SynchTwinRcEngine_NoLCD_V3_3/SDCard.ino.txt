#ifdef SDDATALOGGER
void readSDCardInfos()
{
  if (!card.init(SPI_FULL_SPEED, chipSelect)) {
    Serial << F("SD|0") << endl;
  } 
  else 
  {
    Serial << F("SD|1") << endl;
    waitMs(500);
    // print the type of card
    switch (card.type()) 
    {
      case SD_CARD_TYPE_SD1:
        Serial << F("SD1") << endl;
        break;
      case SD_CARD_TYPE_SD2:
        Serial << F("SD2") << endl;
        break;
      case SD_CARD_TYPE_SDHC:
        Serial << F("SDH") << endl;
        break;
      default:
        Serial << F("SDU") << endl;
    }
//    waitMs(500);
//    if (volume.init(card))
//    {
//      Serial << F("SDF|") << volume.fatType() << endl; 
//    }
  }
  //Serial << card.errorCode() << endl;

}

void listSDdatalog()
{
//  index = 0;
//  static char SDFileName[8];
 // Now we will try to open the 'volume'/'partition' - it should be FAT16 or FAT32
  if (!volume.init(card)) 
  {
    Serial << F("SDN|") << endl;
  }
  else
  {
    root.openRoot(volume);     
    Serial << F("SDL|");
    // list all files in the card with date and size
    root.ls(LS_R | LS_DATE | LS_SIZE);
//    index++;
//    sprintf(SDFileName, "data%03d.txt", index );
//  while (index != 0)
//  {
//    sprintf(SDFileName, "data%03d.txt", index - 1);
//    if (SD.exists(SDFileName) == false) break;
//    index++;
//  }


    
//    if (SD.exists(SDFileName) == false)
//    {
//      Serial << logfilename << endl;
//    }         
  }  
}

void writeSDdatalog(uint16_t y1, uint16_t y2)
{
  // open the file. note that only one file can be open at a time,
  // so you have to close this one before opening another. 
  File myfile = SD.open("datalog.txt", FILE_WRITE);//File myfile = SD.open(SDFileName, FILE_WRITE);
  if (myfile)
  {
    // make a string for assembling the data to log:
    myfile.print(y1);
    myfile.write(',');
    myfile.println(y2);
    myfile.close(); // close the file:
    ledFlashSaveInEEProm(5);// for tests only     
  }    

}

//char updateSDLogName()
//{
////  if (SDFileName == NULL)
////  {
////    sprintf(SDFileName, "data%03d.txt", 0);
////  }
//  static char SDFileName[8];     
//  index = 1;
//  while (index != 0)
//  {
//    sprintf(SDFileName, "data%03d.txt", index - 1);
//    if (SD.exists(SDFileName) == false) break;
//    index++;
//  }
//  index=0;
//  return SDFileName;
//}

//void dumpSDdatalog(String fileName) // cause PB avec VB
//{
//  if (SDCardUsable == true)
//  {
//    char buf[64];
//    File file = SD.open(fileName,FILE_READ);
//    //File endFile = SD.open("copy.txt",FILE_WRITE);
//    Serial.flush();
//    
//    if(file) 
//    {  
//      while(file.position() < file.size()) 
//      {
//      int bytesRead = file.readBytes(buf, sizeof(buf));
//      Serial.println(((float)file.position()/(float)file.size())*100);//progress %
//      // We have to specify the length! Otherwise it will stop when encountering a null byte...
//      Serial.write(buf, bytesRead);//endFile.write(buf, bytesRead); // Send to PC via serial
//      delay(50); 
//      }
//      file.close();
//    }   
//  }
//}

void dumpSDdatalog(char* FileName) { //files available ; TempLog; TimeLog; Settings;
//  Serial.print("begining serial dump of file - ");
//  Serial.println(FileName);
  File DumpFile = SD.open(FileName);
//  if (DumpFile) 
//  {
    while (DumpFile.available()) 
    {
      Serial.write(DumpFile.read());//read until nothing is left
    }
    DumpFile.close();
//  }
//  else {
//    Serial.println("Dump file error.........");
//  }
}//   eg  DumpFile(TEMPLOG.txt) , DumpFile(TIMELOG.txt) , DumpFile(SETTINGS.txt)


void deleteSDdatalog(String fileName)
{
//  if (SDCardUsable == true)
//  {
    // delete the file:
    SD.remove(fileName);
    ledFlashSaveInEEProm(5);    
//  }
}

//void generateFiles() 
//{
////derived from code found at http://forum.arduino.cc/index.php?topic=57460.0
//  String fileName = String();
//  String message = String();
//  unsigned int filenumber = 1;
//  while(!filenumber==0) 
//  {
//    fileName = "datalog";
//    fileName += filenumber;
//    fileName += ".txt";
//    message = fileName;
//    char charFileName[fileName.length() + 1];
//    fileName.toCharArray(charFileName, sizeof(charFileName));
//
//    if (SD.exists(charFileName)) 
//    { 
//      message += " exists.";
//      filenumber++;
//    }
//    else 
//    {
//      File dataFile = SD.open(charFileName, FILE_WRITE);
//      message += " created.";
//      dataFile.close();
//      filenumber = 0;
//    }
//    Serial.println(message);
//  }
//}

#endif


