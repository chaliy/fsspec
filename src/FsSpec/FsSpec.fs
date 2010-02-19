namespace FsSpec

exception AssertFailed of string

module Runner =
    open System.Reflection
    open Microsoft.FSharp.Reflection
    open Microsoft.FSharp.Metadata

    let Run() =
        let asm = System.Reflection.Assembly.GetEntryAssembly()

        let printTitle (title : string) = 
            let sep = (new System.String('-', title.Length))
            printfn "\r\n%s\r\n%s\r\n%s\r\n"  sep title sep 
            
        let fasm = FSharpAssembly.FromAssembly(asm)
        let ee = fasm.Entities
                    |> Seq.filter(fun e -> e.IsModule)
                    |> Seq.filter(fun e -> e.LogicalName.Contains("Spec"))                    
                    |> Seq.filter(fun e -> ( printTitle e.CompiledName ; true ) )
                    |> Seq.collect(fun e -> e.MembersOrValues)
                    |> Seq.filter(fun m -> m.LogicalName.Contains("Describe"))
                    |> Seq.filter(fun m -> ( printfn "\t%s\r\n" m.CompiledName ; true ) )
                    |> Seq.iter(fun m -> (match m.ReflectionMemberInfo with
                                            | :? PropertyInfo as p -> ignore(p.GetValue(null, null))
                                            | :? MethodInfo as m -> ignore(m.Invoke(null, null))
                                         ))                    
        ()
        

[<AutoOpen>]
module SpecHelpers =
           
    let describe desc (f: unit -> unit) =
        printfn "%s" desc
        f()  
        printfn "\r\n"
            
    let it desc (f: unit -> unit) =        
        try
           do f()
           printfn "\t\t- %s - OK" desc
        with
            | AssertFailed x -> printfn "\t\t- %s - %s" desc x
            | x -> printfn "\t\t- %s - %s\r\n%s" desc x.Message (x.ToString())               

    // Asserts...
    type System.Object with       
      member x.should_be_equal_to expected =
        if x <> expected then
           raise (AssertFailed(sprintf "Not really equal. Expected to be %s, but was %s" (expected.ToString()) (x.ToString())))    

      member x.should_not_be_null =                
        if obj.Equals(x, null) then
           raise (AssertFailed(sprintf "Bumm, unexpected NULL"))    

      member x.should_be_null =
        if obj.Equals(x, null) = false then
           raise (AssertFailed(sprintf "Damn! It does not NULL"))    


//           let throw_exception<'a when 'a :> exn> actual =
//  Assert.Throws<'a>(Assert.ThrowsDelegate(actual))
//
//let throws_exception() : unit =
//  raise(System.ArgumentException "Bad things")
// http://weblogs.asp.net/podwysocki/archive/2008/06/04/language-oriented-programming-and-functional-unit-testing-in-f.aspx
