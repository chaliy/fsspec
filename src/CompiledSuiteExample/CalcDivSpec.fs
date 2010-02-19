module ``Calculator Div Operation Sp12ecification``

open FsSpec
open Example.Sut

module ``Describe simple divide opertion`` =   

    let ``divide two integers``() =
        let res = Calc.Div 2 2
        res.should_be_equal_to 1    


    let ``divide by zero (Unexpected Failure)``() =
        let res = Calc.Div 2 0
        res.should_be_equal_to 1
    

    let ``do extended divide with two integers (Unmet Exepectations)``() =
        let res = Calc.DivEx 2 2
        res.should_be_equal_to 1