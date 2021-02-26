import argparse
from subprocess import run, PIPE

def runCommand(command):
    output = run(command.split(), stdout=PIPE)
    return output.stdout.decode('utf-8').strip()


parser = argparse.ArgumentParser(description="Check systemd service")
parser.add_argument("name", help="Name of the service")

group = parser.add_mutually_exclusive_group(required=True)
group.add_argument("-t", "--timer",  help="Timer of the service", action='store_true')
group.add_argument("-s", "--service", help="Status of the service", action='store_true')

args = parser.parse_args()

name = args.name
if args.timer:
    command = f"systemctl show {name}.timer -p ActiveEnterTimestamp -p ActiveState"
    output = runCommand(command).split("\n")
    state = output[0].split("=")[1]
    lastStart = output[1].split("=")[1] 
    print(f"{name}.timer {state}, 
            last started {lastStart if len(lastStart) > 0 else None}")
else:
    command = f"systemctl show {name}.service -p ActiveEnterTimestamp -p User -p Group -p ActiveState"
    output = runCommand(command).split("\n")
    user = output[0].split("=")[1]
    group = output[1].split("=")[1]
    state = output[2].split("=")[1]
    lastStart = output[3].split("=")[1] 
    print(f"{name}.service {state}, 
            user {user if len(user) > 0 else None}, 
            group {group if len(group) > 0 else None} 
            last started {lastStart if len(lastStart) > 0 else None}")

        
