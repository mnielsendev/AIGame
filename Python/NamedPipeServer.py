import time
import win32pipe, win32file, pywintypes


class PipeServer:
    def __init__(self, pipeName):
        self.pipe = win32pipe.CreateNamedPipe(
            r'\\.\pipe\\' + pipeName,
            win32pipe.PIPE_ACCESS_DUPLEX,
            win32pipe.PIPE_TYPE_MESSAGE | win32pipe.PIPE_READMODE_MESSAGE | win32pipe.PIPE_WAIT,
            1, 65536, 65536,
            0,
            None)

    # Careful, this blocks until a connection is established
    def connect(self):
        print('Attempting to connect to Unity...')
        win32pipe.ConnectNamedPipe(self.pipe)
        print('Connected!')

    def read(self):
        print('  Awaiting instruction...')
        while True:
            try:
                while True:
                    message = win32file.ReadFile(self.pipe, 64 * 1024)
                    print(f'Message received: {message}')
                    self.instruct(message)
            except pywintypes.error as e:
                if e.args[0] == 2:
                    print("Could not find pipe. Trying again...")
                    time.sleep(1)
                elif e.args[0] == 109:  # no process on other side OR Pipe closed
                    win32pipe.DisconnectNamedPipe(self.pipe)
                    print("Reconnecting...")
                    win32pipe.ConnectNamedPipe(self.pipe)
            else:
                raise
            time.sleep(0.25)

    def write(self, message):
        print('Sending message...')
        win32file.WriteFile(self.pipe, message.encode() + b'\n')
        print(f'Message delivered: {message}')

    def close(self):
        print('Flushing...')
        win32file.FlushFileBuffers(self.pipe)  # Sometimes returns handle is invalid?
        print('Closing pipe.')
        win32file.CloseHandle(self.pipe)

    def instruct(self, message):
        # Decode bytes into str and separate the instruction command from its data
        msg = message[1].decode('UTF-8').split(':', 1)  # The ':' separates command and associated data
        instr = msg[0]  # The instruction keyword (ie. The message preceding the ':')
        data = msg[1].rsplit('\r', 1)[0]  # The data associated with the instruction
        # POSSIBLE INSTRUCTIONS FOLLOW
        # Send a ping back to Unity to verify connection
        if instr == 'PING':
            print(data)
            self.write('Hello from Python!')
        # Login to NovelAI process
        elif instr == 'LOGIN':  # todo
            print(msg)
        # Request NovelAI generation
        elif instr == 'GENERATE':  # todo
            print(msg)
        else:
            print('Unrecognized instruction.')
        print('  Awaiting further instruction...')



t = PipeServer('UnityPythonNamedPipe')
t.connect()
t.read()
