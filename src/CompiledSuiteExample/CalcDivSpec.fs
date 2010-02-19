module ``Calculator Div Operation Specification``
open FsSpec

module ``Describe simple divide opertion`` =   

    let ``divide two integers`` = spec {
        let res = Calc.Div 2 2
        res.should_be_equal_to 1    
    }


    let ``divide by zero (Unexpected Failure)`` = spec {
        let res = Calc.Div 2 0
        res.should_be_equal_to 1
    }
    
    let ``do extended divide with two integers (Unmet Exepectations)`` = spec {
        let res = Calc.DivEx 2 2
        res.should_be_equal_to 1
    }