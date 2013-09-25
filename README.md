
## Flow Free Solver

This project is a WPF version of my earlier [FlowFreeDlx](https://github.com/taylorjg/FlowFreeDlx "FlowFreeDlx") project.

## Screenshot

![Flow Free Solver Screenshot](https://raw.github.com/taylorjg/FlowFreeSolverWpf/master/Images/Screenshot.png "Flow Free Solver Screenshot")

## Development Steps

* ~~get basic layout with window, grid, controls area~~
* ~~draw fixed size grid~~
* ~~add grid size dropdown and draw dynamically sized grid~~
* ~~add ability to add dots to the grid~~
* ~~add ability to un-add a dot to the grid~~
* ~~pre-populate a 7x7 grid with a puzzle~~
* ~~add ability to draw a path (but without the highlighting of cells)~~
* ~~add a solve button~~
* ~~solve (on UI thread!) + draw solution paths~~
* ~~add highlight to paths~~
* ~~do solve on background thread~~
* ~~display modal dialog box with indeterminate progress bar during solving~~
* ~~add a cancel button to the modal dialog box~~
* ~~add ability to cancel the solve process~~
* ~~add status bar to display stats~~
* ~~add stats re number of rows/cols in the matrix~~
* ~~add stats re time taken to build the matrix~~
* ~~add stats re time taken for Dlx to solve the matrix~~
* ~~add indication of user cancellation (when it applies)~~
* ~~display stats so far even after cancellation ?~~
* ~~add ability to click on the BoardCanvas~~
* ~~raise an event that identifies the cell that was clicked (hit test ?)~~
* ~~add a dropdown with a list of colours, add dot of selected colour when clicking on the board~~
* ~~un-add a dot when clicking on an existing dot~~
* ~~display the colour in the dropdown items (use an item template ?)~~
* add validation of colour pairs on the grid
    * ~~must be exact pairs~~
    * minimum number of pairs for the selected grid size
    * maximum number of pairs for the selected grid size
* ~~add a clear/reset button~~
* ~~pass maxDirectionChanges into BuildMatrixFor()~~
* add ability to change maxDirectionChanges from the UI
* add an app icon
* enhance the validation messages (identify the exact problem)
    * indicate minimum number of pairs if too few
    * indicate maximum number of pairs if too many
    * indicate which colours are not exact pairs
    * any others ?
* ~~use TPL to try to speed up the creation of the matrix ?~~
* Fix the bug introduced by using Parallel.ForEach() (see below)
* apply the MVVM design pattern (MVVM Light Toolkit ?)
    * sub-steps ?

### Bugs

In <code>BuildMatrixFor</code>, we now use <code>Parallel.ForEach</code> to invoke <code>AddInternalDataRowsForColourPair</code> in parallel to speed up the building of the matrix. However, <code>AddInternalDataRowsForColourPair</code> adds items to the <code>internalData</code> list and the <code>_rowIndexToColourPairAndPath</code dictionary. Currently, we do not have any synchronisation around these collections so occassionally the program borks.

Possible solutions:

* Use concurrent collections
* Modify the code so that <code>AddInternalDataRowsForColourPair</code> returns the new items rather than adding them to the collections

I prefer the second option because:

* it feels cleaner
* it should be faster because there is no need to synchronize access to shared resources

