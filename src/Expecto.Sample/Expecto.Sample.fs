module Expecto.Sample

open Expecto.Logging
open Expecto.Logging.Message

module Utils =
  let inline repeat10 f a =
    let mutable v = f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v <- f a
    v
  let inline repeat100 f a = repeat10 (repeat10 f) a
  let inline repeat1000 f a = repeat10 (repeat100 f) a
  let inline repeat10000 f a = repeat10 (repeat1000 f) a

  let logger = Log.create "Sample"

open Utils
open Expecto

// SETUP: Use improved differ from Expecto.Diff
Expecto.Expect.defaultDiffPrinter <- Expecto.Diff.colourisedDiff


let printingFsCheck =
  { FsCheckConfig.defaultConfig with
      receivedArgs = fun _ name no args ->
        logger.debugWithBP (
          eventX "For {test} {no}, generated {args}"
          >> setField "test" name
          >> setField "no" no
          >> setField "args" args)
      maxTest = 128
  }

module ColourisationTests =
  type Weather = {
    Type: string
    Precipitation: string
  }
  type CreditCard = {
    Number: string;
    CCV: string;
    PrintedName: string
  }
  type Person = {
    Name: string
    CreditCard: CreditCard option
    Age: int
  }
  type SillyTestType = {
    A: int
    Nom: string
    Weather: Weather
    Tweets: int
    Person: Person option
  }

  let colourisationTests =
    testList "Colorization tests. These should all fail to diplay the diff" [
      test "Diff for an array with small changes" {
        let personA = {Name = "Kesam"; Age = 30; CreditCard = None}
        let personB = {Name = "Charles"; Age = 42; CreditCard = None}
        Expect.equal ( [personA; personB; personB]) ( [personA; {personB with Name = "Cname"; Age = 104210}; personA]) ""
      }

      test "Diff medium-sized object" {
        Expect.equal
          { A = 12; Nom = "James"; Weather = { Type = "Cloudy"; Precipitation = "2mm" }; Tweets = 101; Person = None}
          { A = 13; Nom = "Bond"; Weather = { Type = "Thunderstorms"; Precipitation = "5mm" }; Tweets = 101; Person = None}
          ""
      }

      test "Diff for big object with many changes" {
        Expect.equal
          { A = 12; Nom = "Cloudy McKesam"; Weather = { Type = "Cloudy"; Precipitation = "2mm" }; Tweets = 101; Person = Some ({Name = "Kesam"; Age = 30; CreditCard = Some ({Number = "892348923498"; CCV = "1232"; PrintedName = "Kesam McLovin"})})}
          { A = 4385; Nom = "Charles Thunder"; Weather = { Type = "Thunderstorms"; Precipitation = "5mm" }; Tweets = 101; Person = Some ({Name = "Charles Cardless"; Age = 30; CreditCard = None}) }
          ""
      }

      test "Multi-lined text with some lines added, removed and modified" {
        let actualText = "
          We the people
          of the united states of america
          establish justice
          ensure domestic tranquility
          provide for the very nice common defence and
          secure the blessing of liberty
          ADDED A LINE IN HERE
          to ourselves and our posterity"

        let expectedText = "
          We the people
          in order to form a more perfect union
          establish justice
          ensure domestic tranquility
          promote the very nice general welfare and
          secure the blessing of liberty
          to ourselves and our posterity
          do ordain and establish this constitution
          for the United States of America"

        Expect.equal actualText expectedText "Highlight added, missing and modified lines/words"
      }
    ]

[<Tests>]
let tests =
  testList "samples" [
    testCase "universe exists (╭ರᴥ•́)" <| fun _ ->
      let subject = true
      Expect.isTrue subject "I compute, therefore I am."

    testCase "when true is not (should fail)" <| fun _ ->
      let subject = false
      Expect.isTrue subject "I should fail because the subject is false"

    testCase "I'm skipped (should skip)" <| fun _ ->
      Tests.skiptest "Yup, waiting for a sunny day..."

    testCase "I'm always fail (should fail)" <| fun _ ->
      Tests.failtest "This was expected..."

    testCase "contains things" <| fun _ ->
      Expect.containsAll [| 2; 3; 4 |] [| 2; 4 |]
                         "This is the case; {2,3,4} contains {2,4}"

    testCase "contains things (should fail)" <| fun _ ->
      Expect.containsAll [| 2; 3; 4 |] [| 2; 4; 1 |]
                         "Expecting we have one (1) in there"

    testCase "Sometimes I want to ༼ノಠل͟ಠ༽ノ ︵ ┻━┻" <| fun _ ->
      Expect.equal "abcdëf" "abcdef" "These should equal"

    test "I am (should fail)" {
      "╰〳 ಠ 益 ಠೃ 〵╯" |> Expect.equal true false
    }

    testCase "You know exns" <| fun _ ->
      failwith "unhandled exception from test code"

    ptestCase "I'm pending" ignore

    // uncomment me:
    //ftestCase "I'm focused, I will cause all other tests to be skipped" <| fun () -> ()

    testSequenced (
      testCase "fn1 faster than fn2" <| fun _ ->
        Expect.isFasterThan (fun () -> repeat10000 log 76.0)
                            (fun () -> repeat10000 log 76.0 |> ignore; repeat10000 log 76.0)
                            "half is faster"
    )

    testCaseAsync "simple, not doing very much" <| async {
      Expect.equal 1 1 "1=1"
      do! async.Zero ()
    }

    // a named group ensures its enclosed Test is run in sequence
    testSequencedGroup "close things" <| testList "closeness" [
      test "Should be close" {
        let actual, expected = 41.5621, 41.5620
        Expect.floatClose Accuracy.medium actual expected "Should be close within 5 sig figs (approx)"
      }

      test "Should not be close enough (should fail)" {
        let actual, expected = 41.562, 41.563
        Expect.floatClose Accuracy.medium actual expected "Should be close within 5 sig figs (approx)"
      }
    ]

    testProperty "addition is commutative" <| fun a b ->
      a + b = b + a

    testPropertyWithConfig printingFsCheck "addition is commutative 2" <| fun a b ->
      a + b = b + a

    testProperty "addition is not commutative (should fail)" <| fun a b ->
      a + b = b + -a

    testPropertyWithConfig FsCheckConfig.defaultConfig "Product is distributive over addition" <|
      // these parameters are generated by FsCheck
      fun a b c ->
        a * (b + c) = a * b + a * c

    ColourisationTests.colourisationTests
  ]

[<EntryPoint>]
let main args =
  runTestsInAssemblyWithCLIArgs [] args