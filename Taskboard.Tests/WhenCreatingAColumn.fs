module ``When creating a column``

open Xunit
open Swensen.Unquote

[<Fact>]
let ``A column is added to the board`` () =
    let state = Given [ BoardCreated { Id = BoardId 1; Name = "Board"; }]
    let command = CreateColumnCommand { Board = BoardId 1; Name = "Column name" } 
    let producedEvents = handle command state
    let expectedEvents = [ ColumnCreated { Name = "Column name"; Id = ColumnId 0 } ]
    test <@ expectedEvents = producedEvents @>

[<Fact>]
let ``Another column is added to the board`` () =
    let state = Given [ BoardCreated { Id = BoardId 1; Name = "Board"; }; ColumnCreated { Name = "Column name"; Id = ColumnId 0 };  ColumnCreated { Name = "Column name"; Id = ColumnId 1 }]
    let command = CreateColumnCommand { Board = BoardId 1; Name = "New column" } 
    let producedEvents = handle command state
    let expectedEvents = [ ColumnCreated { Name = "New column"; Id = ColumnId 2 } ]
    test <@ expectedEvents = producedEvents @>
