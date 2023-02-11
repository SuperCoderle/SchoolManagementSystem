//index
function Switch_Button() {
        var modeSwitch = document.querySelector('.mode-switch');
        modeSwitch.addEventListener('click', function () {
            document.querySelector('.app-container').classList.toggle('dark');
            modeSwitch.classList.toggle('active');
        });
}

function ListView_Button() {
    var listView = document.querySelector('.list-view');
    var gridView = document.querySelector('.grid-view');
    var projectsList = document.querySelector('.project-boxes');

    listView.addEventListener('click', function () {
        gridView.classList.remove('active');
        listView.classList.add('active');
        projectsList.classList.remove('jsGridView');
        projectsList.classList.add('jsListView');
    });
}
function GridView_Button() {
    var listView = document.querySelector('.list-view');
    var gridView = document.querySelector('.grid-view');
    var projectsList = document.querySelector('.project-boxes');
    gridView.addEventListener('click', function () {
        gridView.classList.add('active');
        listView.classList.remove('active');
        projectsList.classList.remove('jsListView');
        projectsList.classList.add('jsGridView');
    });
}

function Show_Message() {
    document.querySelector('.messages-btn').addEventListener('click', function () {
        document.querySelector('.messages-section').classList.add('show');
    });
}

function Close_Message() {
    document.querySelector('.messages-close').addEventListener('click', function () {
        document.querySelector('.messages-section').classList.remove('show');
    });
}
//SaveAsExcelFile
function saveAsExcelFile(filename, byteBase64) {
	var link = document.createElement('a');
	link.download = filename;
	link.href = "data:application/octet-stream;base64," + byteBase64;
	document.body.appendChild(link);
	link.click();
	document.body.removeChild(link);
}

//SaveAsPdfFile
function saveAsPdfFile(filename, bytesBase64) {
    if (navigator.msSaveBlob) {
        var data = window.atob(bytesBase64);
        var bytes = new Uint8Array(data.length);
        for (var i = 0; i < data.length; i++) {
            bytes[i] = data.charCodeAt(i);
        }
        var blob = new Blob([bytes.buffer], { type: "application/octet-stream" });
        navigator.msSaveBlob(blob, filename);
        window.navigator.msSaveOrOpenBlob(blob);
    }
    else {
        var link = document.createElement('a');
        link.download = filename;
        link.href = "data:application/octet-stream;base64," + bytesBase64;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    }
}


//Signup Signin
function SwitchButton() {
    const switchers = [...document.querySelectorAll(".switcher")];
    switchers.forEach((item) => {
        item.addEventListener("click", function () {
            switchers.forEach((item) =>
                item.parentElement.classList.remove("is-active")
            );
            this.parentElement.classList.add("is-active");
            console.log(this.parentElement);
        });
    });
}

