using System;
using System.IO;
using System.Text;
using System.IO.Pipes;
using System.Threading;
using UnityEngine;

public class PipeTest : MonoBehaviour
{
    private void Start () //TODO Put the init of the pipe client thread in a better place than start?
    {
        // Run ClientThread on a separate thread.
        Thread clientThread = new Thread(ClientThread);
        clientThread.Start();
    }

    private void ClientThread ()
	{
		// The "." refers to the local computer
		NamedPipeClientStream pipe = new NamedPipeClientStream(".", "UnityPythonNamedPipe", PipeDirection.InOut);

        StreamReader sr = new StreamReader(pipe);
        StreamWriter sw = new StreamWriter(pipe);

        Debug.Log("Attempting to connect to Python...");
        pipe.Connect();
        Debug.Log("Connected!");
        //Debug.Log(string.Format("There are currently {0} pipe server instances open.", pipe.NumberOfServerInstances));

        Debug.Log("Sending message...");
        Instruct(pipe, Instructions.PING);

        // Read the connection's message
        Debug.Log("Message received: " + sr.ReadLine());


        //pipe.Close();
    }

    private enum Instructions
	{
        PING,
        LOGIN,
        GENERATE
	}

    private void Instruct(NamedPipeClientStream pipe, Instructions instr, string data = null)
	{
        StreamWriter sw = new StreamWriter(pipe);
        switch (instr)
        {
            case Instructions.PING:  // Send a ping to Python to verify connection
                data = "Hello from Unity!";
                break;
            case Instructions.LOGIN:  // Login to NovelAI process
                //TODO
                break;
            case Instructions.GENERATE:  // Request NovelAI generation
                //TODO
                break;
            default:  // This should never happen
                throw new Exception(instr + " is not a valid instruction.");
        }
        string message = (instr.ToString() + ":" + data); // The ":" will be parsed as a separator between command and associated data
        sw.WriteLine(message);
        sw.Flush();
        pipe.WaitForPipeDrain();
        Debug.Log("Message delivered: " + message);
    }


}