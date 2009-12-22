#load "References.fsx"

open FsSpec
open Example.Sut

do describe "Add opertaion of the Calculator" [        
        
        it "should add two integers" (fun unit -> 

            let res = Calc.Add 2 2

            res.should_be_equal_to 4
        );
    ]