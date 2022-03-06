from mimetypes import init
from stat import IO_REPARSE_TAG_MOUNT_POINT
from pyautogui import press, keyUp, keyDown
import pyautogui
pyautogui.PAUSE = 0.000001
import subprocess
import time

threshold = 100
holdlength = 3
lastx = -99
lasty = -88

keyPress = 200

prevx = 0
prevy = 0

def holdfor3(val3):
    sum=0
    #sum = val3[0]+val3[1]+val3[2]
    for i in val3:
        sum +=i
    avg = sum/len(val3)
    if avg > threshold:
        return (1)
    elif avg < (-1*threshold):
        return (-1)
    else:
        return (0)
    
def convert2press(valin,ctrue,cfalse, type):
    if(valin == 1 and type != 1):
        type = 1
        keyDown(ctrue)
        keyUp(cfalse)
    elif (valin == -1 and type != -1):
        type = -1
        keyDown(cfalse)
        keyUp(ctrue)
    elif (valin == 0 and type != 0):
        type = 0
        keyUp(ctrue)
        keyUp(cfalse)

def readJtag():
    holdfor3x = []
    holdfor3y = []
    output = subprocess.Popen('B:\\Quartus\\quartus\\bin64\\nios2-terminal.exe', shell=True, stdout=subprocess.PIPE)
    count = 0
    for i in range(10):
        initial_line = output.stdout.readline()
    while(True):
        initial_line = output.stdout.readline()
        initial_line = initial_line.decode("utf8")
        initial_line = initial_line[:initial_line.find("\r\n")]
        readings = initial_line.split("|",1)
        if(len(initial_line)<11 and len(initial_line)>1):
            # print("x =",readings[0], "|| y =",readings[1])

            x_data = int(readings[0])
            y_data = int(readings[1])
            if(len(holdfor3x)<holdlength):
                holdfor3x.append(x_data)
            else:
                print("x ", holdfor3(holdfor3x))
                lastx = holdfor3(holdfor3x)
                holdfor3x = []
                
            if(len(holdfor3y)<holdlength):
                holdfor3y.append(y_data)
            else:
                print("y ", holdfor3(holdfor3y))
                lasty = holdfor3(holdfor3y)
                holdfor3y = []

            count+=1 
            if (count == keyPress):
                convert2press(lastx, 'a','d', prevx)
                convert2press(lasty, 'w', 's', prevy)
                count = 0
                #while(True): 
                    #try:
                        #output.stdout.readline()
                    #except:
                        #break
        

        #print(type(initial_line))
        #print(len(initial_line)
        #time.sleep(0.0001)
        
#len of negative values is "10", no.of hex bits shown is 8
#len of positive values is "4",  no.of hex bits shown is 2
def main():
    readJtag()
    
if __name__ == '__main__':
    main()