var Now = function () {
    
   
    let d = new Date();
    let y = d.getFullYear();

    let m = (d.getMonth()+1) < 10 ? "0" + (d.getMonth() + 1) : (d.getMonth()+1); 
    let dd = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
    let nextDay = d.getDate() + 1;
    let rang = 4;
    
    
    this.date = y + "/" + m + "/" + dd;
    this.next = y + "/" + m + "/" + nextDay;
    let maxMon = (d.getMonth() + rang);
    if (maxMon > 12) {
        maxMon = maxMon - 12;
        y = y + 1;
    }
    this.maxdate = y + "/" + maxMon + "/" + GetLastDayOfMon(y);
    
    
    this.getNextDay = function (d) {
        let y = d.getFullYear();
        let m = d.getMonth() < 9 ? "0" + (d.getMonth() + 1) : (d.getMonth()+1); 
        let dd = d.getDate() < 10 ? "0" + d.getDate() : d.getDate();
        let nextDay = d.getDate() + 1;
        return y + "/" + m + "/" + nextDay;
    }

    function GetLastDayOfMon(year) {
        var month = maxMon;
        
        if (month == 2) {
            if (IsLeapYear(year)) {
                return "29";
            } else {
                return "28";
            }

        } else if (month == 1 || month == 3 || month == 5 || month == 7
            || month == 8 || month == 10 || month == 12) {
            return "31";

        } else if (month == "04" || month == "06" || month == "09" || month == "11") {
            return "30";
        }
    }
};

function IsLeapYear(year) {
    var isLeap = false;
    if (((year % 4 == 0) && (year % 100 != 0)) || (year % 400 == 0)) {
        isLeap = true;
        return 
    }
    else {
       isLeap = false
    }
    return isLeap;

}



function BonusUpdate() {
    $.ajax({
        type: "POST",
        url: '/Bonus/Update',
        data: null,
        dataType: "json",
        cache: false,

        success: function (xhr) {
            console.log(xhr);
        },
        error: function (xhr) {
            console.log("ERROR");
        }
    });
}

function ExpireDate() {
    $.ajax({
        type: "POST",
        url: '/Order/ExpireDate',
        data: null,
        dataType: "json",
        cache: false,

        success: function (xhr) {
            console.log(xhr);
        },
        error: function (xhr) {
            console.log("ERROR");
        }
    });
}

function BonusNotice() {
    console.log("BonusNotice");
    $.ajax({
        type: "POST",
        url: '/Bonus/Notice',
        data: null,
        dataType: "json",
        cache: false,

        success: function (xhr) {
            console.log(xhr);
        },
        error: function (xhr) {
            console.log("ERROR");
        }
    });
};

function QueryAllPay() {
    $.ajax({
        type: "POST",
        url: '/OrderQuery',
        data: null,
        dataType: "json",
        cache: false,

        success: function (xhr) {
            console.log(xhr);
        },
        error: function (xhr) {
            console.log("ERROR");
        }
    });
}


var FileUploadPost = function (url, arrmodel, callback) {
    console.log('dd');
    let model = arrmodel;
    $.ajax({
        type: "POST",
        url: url,
        headers: { 'x-auth-header': 'r8rEvWpEsK7BMMHc' },
        data: JSON.stringify(arrmodel),
        cache: false,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (xhr) {
            (callback && typeof (callback) === "function") && callback();
        },
        error: function (xhr) {
            $('#message').html('上傳資料過大').show();
            
        }
    });
}