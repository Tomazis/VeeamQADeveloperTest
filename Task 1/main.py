import argparse
import json
import os.path
from subprocess import run, PIPE

def runCommand(command):
    output = run(command.split(), stdout=PIPE)
    return output.stdout.decode('utf-8').strip()

parser = argparse.ArgumentParser(description="Check drive status")
parser.add_argument("path", type=str, help="Path to File")
args = parser.parse_args()

path = args.path
if not os.path.isfile(path):
    print("File doesn't exist")
    exit()
    
with open(path, 'r') as f:
    data = f.readline().rstrip()
if len(data) == 0:
    print("Empty file")
    exit()

command = f'lsblk {data} -J'
output = runCommand(command)

try:
    info = json.loads(output)
except Exception as e:
    print("Wrong device in file")
    exit()

info = info["blockdevices"][0]
# print(f"{*info["type"]}")
if info["mountpoint"]:
   command =  f"df {data} -h"
   out = runCommand(command).split("\n")
   out = out[1].split()
   size = out[3]
   
   command = f'lsblk {data} -fs'
   out = runCommand(command).split("\n")
   out = out[1].split()
   fileSystem = out[1]

   print("{} {} {} {} {} {}".format(data, info["type"], info["size"], size, fileSystem, info["mountpoint"])) 
   
   
else:
    print("{} {} {}".format(data, info["type"], info["size"]))


