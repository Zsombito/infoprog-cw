import subprocess
import time

def hex_to_dec(h): 
    msb_neg = '89abcdef'
    h_dec = int(h, 16)
    if len(h) == 6 and h[0] in msb_neg:
        h_dec -= 2**24
    return h_dec


def readJtag():

    output = subprocess.Popen('C:\\intelFPGA_lite\\18.0\\quartus\\bin64\\nios2-terminal.exe', shell=True, stdout=subprocess.PIPE)
    
    while(True):
        initial_line = initial_line.decode("utf-8")
        line = initial_line
        #[2:][:-5]
        #print(hex_to_dec(line))
        #print(type(line))
        print("this is the initial line", initial_line)
        print("this is the line ", line)
        print("this is the type ",type(line))
        print("this is the length ",len(line))
        # print("this is the int", int(line, 16))
        print("this is the int ",hex_to_dec(line))
        time.sleep(0.5)
        
def main():
    readJtag()

if __name__ == '__main__':
    main()
