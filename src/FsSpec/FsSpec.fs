namespace FsSpec


exception AssertFailed of string

// Struvtures to hold execution state...    

type Result =
        | Pass
        | Fail of string
        | Error of string
   
type Spec = 
    abstract Result : Result
    abstract Description : string   

[<AutoOpen>]
module SpecHelpers =
       
    let private resultString r =
         match r with
            | Result.Pass      -> "OK"
            | Result.Fail msg  -> msg
            | Result.Error msg -> msg      

    let describe desc (specs: Spec seq) =
        printfn "%s" desc
        specs   
        |> Seq.iter (fun (x) -> printfn "\t- %s - %s" x.Description (resultString x.Result))   

    let it desc (f: unit -> unit) =
        let executionResult =
            try
               do f()
               Result.Pass
            with
                | AssertFailed x -> Result.Fail x
                | x -> Result.Error (sprintf "%s\r\n%s" x.Message (x.ToString()))    
        
        { new Spec with
            member this.Result = executionResult
            member this.Description = desc }       

    // Helpers...
    type System.Object with       
      member x.is_equal_to expected =
        if x <> expected then
           raise (AssertFailed("Not really"))
