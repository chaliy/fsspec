module ``Calculator Div Operation Specification``

open FsSpec
open Example.Sut

let ``Describe simple divide opertion`` =

    it "should divide two integers" (fun unit -> 

        let res = Calc.Div 2 2

        res.should_be_equal_to 1
    )

    it "should divide two integers (Unexpected Failure)" (fun unit -> 

        let res = Calc.Div 2 0

        res.should_be_equal_to 1
    )

    it "should divide two integers (Unmet Exepectations)" (fun unit -> 

        let res = Calc.DivEx 2 2

        res.should_be_equal_to 1
    )