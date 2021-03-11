let table1data = document.getElementById('table1data')
let table2data = document.getElementById('table2data')
fetch("https://cosmos-fa.azurewebsites.net/api/dataRetrieval?")
.then(res => res.json())
.then(data => {
    for(let row of data) {
        if(row.deviceId=="esp32")
        table1data.innerHTML += `<tr><td>${row.id}</td><td>${row.deviceId}</td><td>${row.data.epochTime}</td><td>${row.data.temperature}</td><td>${row.data.humidity}</td>`
    else
    table2data.innerHTML += `<tr><td>${row.id}</td><td>${row.deviceId}</td><td>${row.data.epochTime}</td><td>${row.data.temperature}</td><td>${row.data.humidity}</td>`

}
})
