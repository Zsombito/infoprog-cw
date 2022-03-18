from mimetypes import init
from stat import IO_REPARSE_TAG_MOUNT_POINT
from pyautogui import press, keyUp, keyDown
import pyautogui
pyautogui.PAUSE = 0.00000001
import subprocess
import time

threshold = 50
holdlength = 3
lastx = -99
lasty = -88

keyPress = 200

prevx = 0
prevy = 0
prevx_shift = 0
prevy_shift = 0

def holdfor3(val3):
    sum=0
    #sum = val3[0]+val3[1]+val3[2]
    for i in val3:
        sum +=i
    avg = sum/len(val3)
    if avg > (4*threshold):
        return (2)
    elif avg < (-4*threshold):
        return (-2)
    elif avg > threshold:
        return (1)
    elif avg < (-1*threshold):
        return (-1)
    else:
        return (0)
    
def convert2press(valin,cpos,cneg, shift, valin_prev, valshift_prev):
    if(valin == 1 and valin_prev != 1):
        valin_prev = 1
        keyDown(cpos)
        keyUp(cneg)
    elif (valin == -1 and valin_prev != -1):
        valin_prev = -1
        keyUp(cpos)
        keyDown(cneg)
    elif (valin == 0):
        valin_prev = 0
        keyUp(cneg)
        keyUp(cpos)

    if((valin == 2 or valin == -2) and valshift_prev != 1):
        valshift_prev = 1
        keyDown(shift)
    elif (valin != 2 and valin != -2):
        valshift_prev = 0
        keyUp(shift)

def readJtag():
    holdfor3x = []
    holdfor3y = []
    output = subprocess.Popen('C:\\intelFPGA_lite\\18.1\\quartus\\bin64\\nios2-terminal.exe', shell=True, stdout=subprocess.PIPE)
    count = 0
    for i in range(10):
        initial_line = output.stdout.readline()
    while(True):
        initial_line = output.stdout.readline()
        initial_line = initial_line.decode("utf8")
        initial_line = initial_line[:initial_line.find("\r\n")]
        readings = initial_line.split("|",2)
        if(len(initial_line)<11 and len(initial_line)>1):
            # print("x =",readings[0], "|| y =",readings[1])

            x_data = int(readings[0])
            y_data = int(readings[1])
           # print(readings[2])
            if(len(holdfor3x)<holdlength):
                holdfor3x.append(x_data)
            else:
                #print("x ", holdfor3(holdfor3x))
                lastx = holdfor3(holdfor3x)
                holdfor3x = []
                
            if(len(holdfor3y)<holdlength):
                holdfor3y.append(y_data)
            else:
                #print("y ", holdfor3(holdfor3y))
                lasty = holdfor3(holdfor3y)
                holdfor3y = []

            count+=1 
            if (count == keyPress):
                convert2press(lastx, 'a','d', 'f', prevx, prevx_shift)
                convert2press(lasty, 's', 'w', 'f',prevy, prevy_shift)
                if (int(readings[2])==1):
                    press('Q')
                count = 0
                #while(True): 
                    #try:
                        #output.stdout.readline()
                    #except:
                        #break
        

        #print(valin_prev(initial_line))
        #print(len(initial_line)
        #time.sleep(0.0001)
        
#len of negative values is "10", no.of hex bits shown is 8
#len of positive values is "4",  no.of hex bits shown is 2
def main():
    readJtag()
    
if __name__ == '__main__':
    main()