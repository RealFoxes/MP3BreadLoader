function loadSounds() {
    $.ajax({
        url: apiBasePath + "api/audiocontent",
        type: 'GET',
        dataType: 'json',
        success: function (res) {
            drawSounds(res.reverse());
        }
    });
}

function drawSounds(sounds) {
    sounds.forEach(function (elem) {
        var isFamilyFilter = elem.x ? "Нет" : "Да";
        const html = `
        <div id="${elem.id}" class="sound card col-12">
            <h2>${elem.short}</h2>
            <p><b>Полное название:</b> <span>${elem.name}</span></p>
            <p><b>Семейный фильтр:</b> <span>${isFamilyFilter}</span></p>
            <div class="row justify-content-between">
                <div class="play p-2"><button type="button" class="btn btn-primary" onclick="loadAudio(${elem.id})">Загрузить звук</button></div>
                <div class="delete p-2"><button type="button" class="btn btn-secondary" onclick="deleteAudio(${elem.id})">Удалить звук</button></div>
            </div>
        </div>
        `
        $("#sounds").append(html);
    });
}

function loadAudio(id) {
    const html = `
    <audio controls="">
        <source src="${apiBasePath + 'api/audiocontent/' + id}" type="audio/ogg">
    :( похоже ваш браузер не поддерживает воспроизведение ogg файлов.
    </audio>
    `
    $(`#sounds #${id} .play`).html(html);
}

function deleteAudio(id) {
    const confirmed = window.confirm('Вы уверены что хотите удалить звук?');
    if (confirmed) {
        $.ajax({
            url: apiBasePath + "api/audiocontent/" + id,
            type: 'DELETE',
            dataType: 'json',
            complete: function (res) {
                $("#sounds").html('');
                loadSounds();
            }
        });
    }
}
