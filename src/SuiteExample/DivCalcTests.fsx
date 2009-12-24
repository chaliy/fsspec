#load "References.fsx"

open FsSpec
open Example.Sut

do describe "Devide opertion for the Calculator" [        

    it "should devide two integers" (fun unit -> 

        let res = Calc.Div 2 2

        res.should_be_equal_to 1
    );

    it "should devide two integers (Unexpected Failure)" (fun unit -> 

        let res = Calc.Div 2 0

        res.should_be_equal_to 1
    );

    it "should devide two integers (Unmet Exepectations)" (fun unit -> 

        let res = Calc.DivEx 2 2

        res.should_be_equal_to 1
    );
]