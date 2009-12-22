namespace FsSpec


exception AssertFailed of string

// Structures to hold execution state...    

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
        printfn desc
        specs   
        |> Seq.iter (fun (x) -> printfn "\t- %s - %s" x.Description (resultString x.Result))   
        printfn "\r\n"

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

    // Asserts...
    type System.Object with       
      member x.should_be_equal_to expected =
        if x <> expected then
           raise (AssertFailed(sprintf "Not really equal. Expected to be %s, but was %s" (expected.ToString()) (x.ToString())))    


//           let throw_exception<'a when 'a :> exn> actual =
//  Assert.Throws<'a>(Assert.ThrowsDelegate(actual))
//
//let throws_exception() : unit =
//  raise(System.ArgumentException "Bad things")
// http://weblogs.asp.net/podwysocki/archive/2008/06/04/language-oriented-programming-and-functional-unit-testing-in-f.aspx
