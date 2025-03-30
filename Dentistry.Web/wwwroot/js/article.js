function copyToClipboard() {
    navigator.clipboard.writeText(window.location.href).then(() => {
        alert("Link đã được sao chép!");
    }).catch(err => {
        console.error('Copy failed', err);
    });
}
function copyAndOpenInstagram() {
    const url = window.location.href;
    navigator.clipboard.writeText(url).then(() => {
        alert("Link đã được sao chép! Mở Instagram để đăng bài.");
        window.open("https://www.instagram.com/", "_blank");
    }).catch(err => {
        console.error("Copy failed", err);
    });
}
function printArticle() {
    var articleContent = document.querySelector("article").innerHTML; // Lấy nội dung trong <article>
    var printWindow = window.open("", "_blank"); // Mở cửa sổ mới để in

    printWindow.document.write(`
            <html>
            <head>
                <title>In Bài Viết</title>
                <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css">
                <style>
                    body { font-family: Arial, sans-serif; padding: 20px; }
                    article { max-width: 800px; margin: auto; }
                    .d-flex { display: none; } /* Ẩn nút chia sẻ */
                </style>
            </head>
            <body onload="window.print(); window.close();">
                <article>${articleContent}</article>
            </body>
            </html>
        `);

    printWindow.document.close(); // Đóng tài liệu sau khi ghi
}