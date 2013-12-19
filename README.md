MemorySnapshotTool
==================

A Unity3D project with an enclosed tool for serializing current memory objects.  Useful for leak detection.

The backstory:
Unity's memory profiler has come a long way in the last few versions.  But it still lacks a way to record a snapshot of the objects in memory at a certain time for later comparison.  This project shows the implementation of a tool that can record the Unity objects in memory in a format that makes it easy to compare against other snapshots.


Using:
Run the scene entitled "Example" and a button will appear that allows the developer to capture a snapshot of all unity memory objects.  This data will be organized by Object type and then by object name and serialized to a file.  In this example, the files will be written to a new folder in Assets entitled "MemorySnapshots".