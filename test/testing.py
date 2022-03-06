from mimetypes import init
from stat import IO_REPARSE_TAG_MOUNT_POINT
import subprocess
import time

threshold = 50

def hex_to_dec(h): 
    msb_neg = '89abcdef'
    h_dec = int(h, 16)
    if len(h) == 6 and h[0] in msb_neg:
        h_dec -= 2**24
    return h_dec

def holdfor3(val3):
    sum = val3[0]+val3[1]+val3[2]
    avg = sum/3
    if avg > threshold:
        return (1)
    elif avg < (-1*threshold):
        return (-1)
    else:
        return (0)
    


def readJtag():
    holdfor3x = []
    holdfor3y = []
    output = subprocess.Popen('C:\\intelFPGA_lite\\18.0\\quartus\\bin64\\nios2-terminal.exe', shell=True, stdout=subprocess.PIPE)
    
    while(True):
        initial_line = output.stdout.readline()
        initial_line = initial_line.decode("utf8")
        initial_line = initial_line[:initial_line.find("\r\n")]
        readings = initial_line.split("|",1)
        if(len(initial_line)<11 and len(initial_line)>1):
            # print("x =",readings[0], "|| y =",readings[1])

            x_data = int(readings[0])
            y_data = int(readings[1])
            if(len(holdfor3x)<3):
                holdfor3x.append(x_data)
            else:
                print("x ", holdfor3(holdfor3x))
                holdfor3x = []
                

            if(len(holdfor3y)<3):
                holdfor3y.append(y_data)
            else:
                print("y ", holdfor3(holdfor3y))
                holdfor3y = []

        #print(type(initial_line))
        #print(len(initial_line)
        #time.sleep(0.0001)
        
#len of negative values is "10", no.of hex bits shown is 8
#len of positive values is "4",  no.of hex bits shown is 2
def main():
    readJtag()

if __name__ == '__main__':
    main()