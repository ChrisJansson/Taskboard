module ``When moving a column``

open Xunit
open Swensen.Unquote
open System

[<Fact>]
let ``Column is moved`` () =
    let state = Given [ BoardCreated { Name = "Board"; Id = BoardId 1; };  ColumnCreated { Name = "Column 0"; Id = ColumnId 0; State = StateId 0}; ColumnCreated { Name = "Column 1"; Id = ColumnId 1; State = StateId 0} ]
    let command = MoveColumnCommand { Id = ColumnId 0; TargetPosition = 1}
    let actualEvents = handle command state 
    let expectedEvents = [ ColumnMoved { Id = ColumnId 0; Position = 1 }]
    test <@ expectedEvents = actualEvents @>

[<Fact>]
let ``Column cannot be moved to negative positions`` () =
    let state = Given [ BoardCreated { Name = "Board"; Id = BoardId 1; };  ColumnCreated { Name = "Column 0"; Id = ColumnId 0; State = StateId 0}; ColumnCreated { Name = "Column 1"; Id = ColumnId 1; State = StateId 0} ]
    let command = MoveColumnCommand { Id = ColumnId 0; TargetPosition = -1 }
    let actualEvents () = handle command state  |> ignore
    Assert.Throws<Exception>(Action actualEvents)

[<Fact>]
let ``Column cannot be moved to positions higher than the last positoin`` () =
    let state = Given [ BoardCreated { Name = "Board"; Id = BoardId 1; };  ColumnCreated { Name = "Column 0"; Id = ColumnId 0; State = StateId 0}; ColumnCreated { Name = "Column 1"; Id = ColumnId 1; State = StateId 0} ]
    let command = MoveColumnCommand { Id = ColumnId 0; TargetPosition = 2 }
    let actualEvents () = handle command state  |> ignore
    Assert.Throws<Exception>(Action actualEvents)

[<Fact>]
let ``Cannot move unknown column`` () =
    let state = Given [ BoardCreated { Name = "Board"; Id = BoardId 1; };  ColumnCreated { Name = "Column 0"; Id = ColumnId 0; State = StateId 0}; ]
    let command = MoveColumnCommand { Id = ColumnId 1; TargetPosition = 0 }
    let actualEvents () = handle command state  |> ignore
    Assert.Throws<Exception>(Action actualEvents)
