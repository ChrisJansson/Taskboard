module ``When creating a column``


open Xunit
open Swensen.Unquote

[<Fact>]
let ``A column is added to the board`` () =
    let state = Given [ BoardCreated { Id = BoardId 1; Name = "Board"; }]
    let command = CreateColumnCommand { Board = BoardId 1; Id = ColumnId 1; Name = "Column name" } 
    let producedEvents = handle command state
    let expectedEvents = [ ColumnCreated { Name = "Column name" } ]
    test <@ expectedEvents = producedEvents @>
