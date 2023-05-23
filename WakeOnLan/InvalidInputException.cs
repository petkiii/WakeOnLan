﻿namespace WakeOnLan;

public class InvalidInputException : Exception
{
    public string? Input { get; }

    public InvalidInputException(string message, string? input) : base(message)
    {
        Input = input;
    }
}