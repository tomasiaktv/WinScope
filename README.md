![Platform](https://img.shields.io/badge/platform-Windows-blue)
![Language](https://img.shields.io/badge/language-C%23-blueviolet)
![Framework](https://img.shields.io/badge/.NET-8.0-purple)
![UI](https://img.shields.io/badge/UI-WPF-informational)
![License](https://img.shields.io/badge/license-MIT-green)

WinScope

WinScope is a Windows desktop process monitoring application built with C#, WPF, and the MVVM pattern.
It provides real-time insight into running system processes, including CPU usage, memory consumption, and core process metadata, while emphasizing clean architecture, explainable systems logic, and responsive UI design.

This project was built as a portfolio piece to demonstrate Windows-focused engineering, asynchronous programming, and systems-level reasoning rather than visual polish or feature bloat.

Features

Real-time process list with automatic refresh

Per-process CPU usage (sampling-based calculation)

Memory usage (working set)

Process metadata (name, PID)

Selection preserved across refresh cycles

Responsive UI with no blocking operations

Graceful handling of access-restricted processes

How CPU Usage Is Calculated

Windows does not provide instantaneous CPU percentage per process.
WinScope computes CPU usage by sampling CPU time over intervals:

Capture each process’s TotalProcessorTime

Wait for a fixed interval

Capture CPU time again

Calculate the delta CPU time used

Divide by elapsed wall-clock time

Normalize by logical processor count

This produces a stable, explainable CPU percentage and mirrors how professional monitoring tools operate.

Architecture Overview

WinScope follows a layered design with clear separation of concerns:

Models (WinScope.Models)

Defines immutable data models used across the application.

ProcessInfo represents a snapshot of a running process

Includes raw system data and computed display properties

Core (WinScope.Core)

Defines abstractions and contracts.

IProcessSnapshotProvider describes how process data is retrieved

UI depends only on interfaces, not implementations

Platform (WinScope.Platform)

Implements system-specific logic using Windows APIs.

Uses System.Diagnostics.Process

Handles CPU sampling, memory collection, and exception safety

Encapsulates all OS interaction

App (WinScope.App)

Contains the WPF UI and ViewModels.

MainViewModel manages refresh logic and state

Uses async/await with cancellation

MainView is a pure UI UserControl with bindings only

MainWindow hosts the application and manages lifetime

This structure keeps the codebase maintainable, testable, and extensible.

Technology Stack

C#

.NET 8

WPF

MVVM

async/await

Windows Diagnostics APIs

xUnit (test project scaffolded)

Running the Project

Clone the repository

Open WinScope.sln in Visual Studio

Ensure .NET 8 is installed

Build and run the WinScope.App project

The application will start monitoring processes automatically.

Design Goals

Demonstrate systems-level thinking without unsafe or native code

Favor clarity and explainability over clever tricks

Keep UI logic separate from system logic

Handle real-world OS constraints gracefully

Reflect professional Windows tooling patterns

Future Improvements

Sorting and filtering by CPU or memory usage

Threshold-based highlighting (resource spikes)

Historical snapshots

GPU metrics (best-effort, hardware-dependent)

Additional unit tests around sampling logic

Why This Project Exists

WinScope was built to bridge the gap between academic coursework and real-world systems development.
It focuses on how Windows actually exposes process information, how monitoring tools derive metrics, and how to design software that is robust under real operating system constraints.
