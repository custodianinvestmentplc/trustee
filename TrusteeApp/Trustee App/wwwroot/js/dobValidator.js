$.validator.addMethod('DateOfBirth', function (value, element, params) {

    let year = params[1], date = new Date(value);

    return date.getUTCFullYear() <= year;
});

$.validator.unobtrusive.adapters.add('DateOfBirth', ['year'], function (options) {

    let element = $("#DateOfBirth");

    options.rules['DateOfBirth'] = [element, parseInt(options.params['year'])];
    options.messages['DateOfBirth'] = options.message;
});





    //console.log(params)
    //console.log(element)
    //console.log(value)

    //console.log(getMaxYear())
    //console.log(options)

    //options.messages['dob'] = `Applicant must be above the age of 18`;

//function getMaxYear() {

//    const today = new Date();

//    const currentYear = today.getUTCFullYear();
//    const maxYear = currentYear - 18;

//    return maxYear;
//}



//function getMaxDate() {

//    const today = new Date();

//    const currentYear = today.getFullYear();
//    const maxYear = currentYear - 18;

//    const currentMonth = today.getMonth();
//    const currentDay = today.getDate();

//    const maxDate = new Date(maxYear, currentMonth, currentDay).toISOString();

//    return maxDate;
//}


















//$.validator.unobtrusive.adapters.add('restrictmaxdates', ['maxdate'], function (options) {

//    const today = new Date();

//    const currentYear = today.getFullYear();
//    const maxYear = currentYear - 18;

//    const currentMonth = today.getMonth();
//    const currentDay = today.getDate();
//    const todayToISO = today.toISOString();

//    const maxDate = new Date(maxYear, currentMonth, currentDay).toISOString();


//    options.rules['restrictmaxdates'] = { maxdate: maxDate };
//    options.messages['restrictmaxdates'] = `Applicant must be above the age of 18`;
//});

//$.validator.addMethod("restrictmaxdates", function (value, element, param) {

//    var date = new Date(value);

//    var maxDate = new Date(param.maxdate);

//    return date <= maxDate;

//});