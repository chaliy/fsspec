module ``Calculator Add Operation Specification``

open FsSpec
open Example.Sut    

let ``Describe simple add cases`` = 
    it "should add two integers" (fun unit -> 

        let res = Calc.Add 2 2

        res.should_be_equal_to 4
    )