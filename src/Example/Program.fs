open FsSpec

// System Under Test 
module Calc =
    let Add i1 i2 = 
        i1 + i2

    let Div i1 i2 = 
        i1 / i2

    let DivEx i1 i2 = 
        i1 + 1 / i2

// Tests...
do describe "Calculator" (fun unit ->
    it "should add two integers" (fun unit ->
        
        let res = Calc.Add 2 2

        res.should_be_equal_to 4
    )

    it "should devide two integers" (fun unit -> 

        let res = Calc.Div 2 2

        res.should_be_equal_to 1
    )

    it "should devide two integers (Unexpected Failure)" (fun unit -> 

        let res = Calc.Div 2 0

        res.should_be_equal_to 1
    )

    it "should devide two integers (Unmet Exepectations)" (fun unit -> 

        let res = Calc.DivEx 2 2

        res.should_be_equal_to 1
    )
)