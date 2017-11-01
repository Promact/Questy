export const FakeTest = {
    'id': 2002,
    'createdDateTime': '2017-10-04T11:21:01.054085',
    'testName': 'Hello',
    'link': 'hjxJ4cQ2fI',
    'browserTolerance': 0,
    'startDate': '2017-10-04T11:50:00',
    'endDate': '2017-10-07T11:21:00',
    'duration': 60,
    'warningTime': 5,
    'focusLostTime': 5,
    'warningMessage': 'Your test is about to end. Hurry up!!',
    'correctMarks': 1,
    'incorrectMarks': 0,
    'isPaused': false,
    'isLaunched': true,
    'questionOrder': 1,
    'optionOrder': 1,
    'allowTestResume': 0,
    'categoryAcList': null,
    'testIpAddress': [],
    'numberOfTestAttendees': 18,
    'numberOfTestSections': 1,
    'numberOfTestQuestions': 3,
    'createdByUserId': '654cc152-b54c-49a2-b9d6-89ba9140e545'
}

export const FakeAttendee = {
    'id': 1,
    'email': 'fakeattendee@fakesite.fakenet',
    'firstName': 'fake',
    'lastName': 'u',
    'contactNumber': '0000000000',
    'rollNumber': 'FAKE-0',
    'attendeeBrowserToleranceCount': 0
}

//Don't add any new question here until you have modified unit test
export const FakeTestQuestions = [{
    "question": {
        "question": {
            "id": 7008,
            "questionDetail": "Programming Question",
            "questionType": 2,
            "difficultyLevel": 0,
            "categoryID": 6,
            "isSelect": false
        }, "singleMultipleAnswerQuestion": null,
        "codeSnippetQuestion": {
            "checkCodeComplexity": true,
            "checkTimeComplexity": true,
            "runBasicTestCase": true,
            "runCornerTestCase": true,
            "runNecessaryTestCase": true,
            "languageList": ["C", "Cpp", "Java"],
            "codeSnippetQuestionTestCases": []
        }
    }, "questionStatus": 3
},
    {
        "question": {
            "question": {
                "id": 8008,
                "questionDetail": "<p>WAP to echo</p>\n",
                "questionType": 2,
                "difficultyLevel": 0,
                "categoryID": 6,
                "isSelect": false
            }, "singleMultipleAnswerQuestion": null,
            "codeSnippetQuestion": {
                "checkCodeComplexity": false,
                "checkTimeComplexity": false,
                "runBasicTestCase": true,
                "runCornerTestCase": true,
                "runNecessaryTestCase": true,
                "languageList": ["C", "Cpp"],
                "codeSnippetQuestionTestCases": []
            }
        }, "questionStatus": 3
    }]

export const FakeTestLogs = {
    'visitTestLink': new Date('Wed Oct 11 2017 06:53:13 GMT+0530 (India Standard Time)'),
    'fillRegistrationForm': new Date('Wed Oct 11 2017 06:53:13 GMT+0530 (India Standard Time)'),
    'startTest': new Date('Wed Oct 11 2017 06:53:15 GMT+0530 (India Standard Time)'),
    'finishTest': new Date('Tue Oct 17 2017 09:08:45 GMT+0530 (India Standard Time)'),
    'resumeTest': new Date('Wed Oct 11 2017 06:57:04 GMT+0530 (India Standard Time)'),
    'awayFromTestWindow': new Date('Wed Oct 11 2017 07:00:31 GMT+0530 (India Standard Time)')
}

export const FakeCodeResponse = {
    'message': 'Success',
    'error': '',
    'errorOccurred': false
}

export const FakeResumeData = [{
    'questionId': 100,
    'optionChoice': [1, 2],
    'code': '',
    'questionStatus': 1,
    'isAnswered': 'true'
}]