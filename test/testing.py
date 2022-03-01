import subprocess
import time

def hex_to_dec(h): 
    msb_neg = '89abcdef'
    h_dec = int(h, 16)
    if len(h) == 6 and h[0] in msb_neg:
        h_dec -= 2**24
    return h_dec


def readJtag():
    counter = 0
    output = subprocess.Popen('B:\\Quartus\\quartus\\bin64\\nios2-terminal.exe', shell=True, stdout=subprocess.PIPE)
    x_data = 0
    y_data = 0
    z_data = 0
    while(True):
        initial_line = output.stdout.readline()
        if len(initial_line) < 14:
            """
            line = output.stdout.readline()
            line = line.decode("utf-8")

            line1 = output.stdout.readline()
            line1 = line1.decode("utf-8")
            
            line2 = output.stdout.readline()
            line2 = line2.decode("utf-8")
            """
            if counter>3:
                counter = 1
            counter = counter + 1

            line = output.stdout.readline()
            line = line.decode("utf-8")
            

            if line[0] == "x":
                x_data = line[2:]
            elif line[0] == "z":
                z_data = line[2:]
            elif line[0] == "y":
                y_data = line[2:]
            
            if counter==3:
                print("x=",x_data,"y=",y_data, "z=", z_data)
                time.sleep(0.5)
            #line = output.stdout.readline()
            #line = line.decode("utf-8")
            #y_data = line

            #line = output.stdout.readline()
            #line = line.decode("utf-8")
            #z_data = line
            
            #if(x_data):
                #print("this is the line ", x_data)
                #print("this is the int", int(x_data, 16), " in our func", hex_to_dec(x_data))
            #print(hex_to_dec(line))
            #print("this is the type ",type(line))
            #print("this is the length ",len(line))
            #time.sleep(0.5)


        #initial_line = str(output.stdout.readline())
        #if len(initial_line) > 14 or len(initial_line) < 8:
            #continue
        #else:
            #line = initial_line[2:][:-5]
            #print(hex_to_dec(line))
            #print(type(line))
            
            #print("this is the initial line", initial_line)
            
            # print("this is the int", int(line, 16))
            #print("this is the int ",int(line, 16))
        
        
def main():
    readJtag()

if __name__ == '__main__':
    main()
