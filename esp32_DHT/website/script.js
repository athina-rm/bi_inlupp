let table1data = document.getElementById('table1data')

fetch("https://forcosmos.azurewebsites.net/api/dataRetrieval?")
.then(res => res.json())
.then(data => {
    for(let row of data) {       
        table1data.innerHTML += `<tr><td>${row.id}</td><td>${row.deviceId}</td><td>${row.measurementType}</td><td>${row.measurementTime}</td><td>${row.data.Temperature}</td><td>${row.data.RelativeHumidity}</td><td>${row.data.WindSpeed}</td>`

    }
})                     
let table2data = document.getElementById('table2data')
fetch("https://tablestorage-fa.azurewebsites.net/api/DataretreivalTS?code=jsEZ4U2H0h6cs7aRGiyRVaBasrqTvITRfHr5YCOEPJ4dB0GBaoni4g==")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        table2data.innerHTML += `<tr><td>${row.rowKey}</td><td>${row.partitionKey}</td><td>${row.measurementType}</td><td>${row.measurementTime}</td><td>${row.temperature}</td><td>${row.humidity}</td>`
    }
})    