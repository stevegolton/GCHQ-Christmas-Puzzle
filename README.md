# GCHQ-Christmas-Puzzle
Programmatic solution to the GCHQ Christmas puzzle.

Summary
-------------------------------
This application is written in C# and was developed under Windows using Visual Studio 2013. The application presents a WinForms window which contains the controls and displays the puzzle in it's various different states as it is being solved.

This solver uses a sort-of brute force algorithm drawn from doing the method I initally thought of for how I would takle the puzzle manually with pen and paper, except that a computer can do it much more quickly!

Key to cell colours
 -  Black - definitely set
 -  Pink - definitely clear
 -  Shades of blue - uncertain... the darker the blue the more likely the cell is to be set.

Build & Run Instructions
-----------------------------
On Windows open the solution in at least Microsoft Visual Studio 2013. Build and run in release mode. Press the button to run a single iteration. The algorithm is calculated in a background thread so the UI can update as the puzzle is being solved.

As far as I know there is no way to run this on Linux as Winforms are incompatible with Mono :(...

Algorithm
----------------------------
Each cell in the 25x25 grid can be in either a set or a cleared state. For each row all the possible combinations of set and unset are considered which match the row's rule. Any cell in the row which is set for all combinations is marked as "defintite" (black) and any cell which is left clear for all combinations is marked as "definitely not" (pink). This process is then repeated for each of the columns. Any combination which does not fit with the existing "definite" and "defintely not" cells is disregarded. A pass over the rows followed by the colums is one iteration. The method can find the solution to the GCHQ puzzle in around 4 iterations (the code scans after 3 iterations due to the QR code's reundancy).
