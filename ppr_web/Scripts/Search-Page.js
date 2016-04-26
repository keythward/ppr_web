function showDublinDropdown()
{
    $("#areaSelectBox").val('');
    if ($("#counties-dropdown option:selected").text() == "Dublin") {
        $("#toggle-postcode-dropdown").show();
        if ($("#year-dropdown option:selected").text() == "2016") {
            $("#current-dates-dropdown1").hide();
            $("#current-dates-dropdown2").show();
            $("#dates-dropdown2").hide();
            $("#dates-dropdown1").hide();
        }
        else {
            $("#current-dates-dropdown1").hide();
            $("#current-dates-dropdown2").hide();
            $("#dates-dropdown1").hide();
            $("#dates-dropdown2").show();
        }
    }
    else {
        $("#toggle-postcode-dropdown").hide();
        if ($("#year-dropdown option:selected").text() == "2016") {
            $("#current-dates-dropdown1").show();
            $("#current-dates-dropdown2").hide();
            $("#dates-dropdown2").hide();
            $("#dates-dropdown1").hide();
        }
        else {
            $("#current-dates-dropdown1").hide();
            $("#current-dates-dropdown2").hide();
            $("#dates-dropdown1").show();
            $("#dates-dropdown2").hide();
        }
    }
}

function searchButtonOnDublinClick()
{
    if ($("#counties-dropdown option:selected").text() == "Dublin")
    {
        $("#toggle-postcode-dropdown").show();
    }
    else {
        $("#toggle-postcode-dropdown").hide();
    }
}

function showProperDatesForCurrentYear()
{
    if ($("#year-dropdown option:selected").text() == "2016") {
        if ($("#counties-dropdown option:selected").text() == "Dublin") {
            $("#current-dates-dropdown1").hide();
            $("#current-dates-dropdown2").show();
            $("#dates-dropdown2").hide();
            $("#dates-dropdown1").hide();
        }
        else {
            $("#current-dates-dropdown1").show();
            $("#current-dates-dropdown2").hide();
            $("#dates-dropdown1").hide();
            $("#dates-dropdown2").hide();
        }
    }
    else {
        $("#current-dates-dropdown1").hide();
        $("#current-dates-dropdown2").hide();
        showDublinDropdown();
    }
}
