import socket
import re
from Crypto.Util.number import *

hostname = "0.0.0.0"
port = 11111

def byteToString(byte):
    text = ''
    for i in byte:
        text += chr(i)
    return text

def hamming_decode(y1,y2,y3,y4,y5,y6,y7):
    """  Hamming  decoding  of the 7 bits  signal  """
    b1= (y1+y3+y5+y7) % 2
    b2= (y2+y3+y6+y7) % 2
    b3= (y4+y5+y6+y7) % 2
    b=4*b3+2*b2+b1 
    # the  integer  value
    if b==0 or b==1 or b==2 or b==4:
        return (y3,y5,y6,y7)
    else:
        y=[y1,y2 ,y3,y4,y5 ,y6,y7]
        y[b-1]=(y[b-1]+1) % 2   # correct  bit b
        return (y[2],y[4],y[5],y[6])
    
def byteToString(byte):
    text = ''
    for i in byte:
        text += chr(i)
    return text

s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
s.connect((hostname, port))

print(s.recv(1024).decode())
print(s.recv(1024).decode())
temp = 0
flag = []
for i in range(68):
    flag.append({})

while temp < 100:
    temp += 1
    data = s.recv(1024).decode()
    pattern = re.compile("[01]{7}")
    hamming = pattern.findall(data)
    answer = b''
    for data in hamming:
        ready = []
        for i in data:
            ready.append(int(i))
        resp = hamming_decode(*ready)
        for i in resp:
            if i == 1:
                answer += b"1"
            else:
                answer += b"0"
    try:
        signal = byteToString(long_to_bytes(int(hex(int(answer, 2)),16)))
        for i in range(len(signal)):
            if signal[i].isalpha() or signal[i].isdigit() or signal[i] == '_' or signal[i] == '{' or signal[i] == '}':
                if signal[i] in flag[i]:
                    flag[i][signal[i]] += 1
                else:
                    flag[i][signal[i]] = 1
        for dic in flag:
            print(max(dic, key=dic.get), end='')
            a = 'a'
            for l in range(1000):
                a = a + 'a'
        print("\n")

    except:
        continue