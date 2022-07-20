const channelID = "UCECu8pY7fhH3-orl-VIWyrA";
const reqURL = "https://www.youtube.com/feeds/videos.xml?channel_id=";

$.getJSON("https://api.rss2json.com/v1/api.json?rss_url=" + encodeURIComponent(reqURL) + channelID, function (data) {
    let items = data.items;
    for (let index = 0; index < items.length; index++) {
        const element = items[index];

        let link = data.items[index].link;
        let id = link.substr(link.indexOf("=") + 1);
        $(`#youtube_video-${index}`).attr("src", "https://youtube.com/embed/" + id + "?controls=0&showinfo=0&rel=0");
    }
});