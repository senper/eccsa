import { downloadLaw } from "../../pages/api/laws";

export default function LawsList({
  laws,
  getLocal,
  getLocalByVal,
  showNotification,
  locale,
  setErrorMsg
}) {
  function downloadDoc(law) {
    downloadLaw(law.id, locale != "en")
      .then((data) => {
        const url = data.request.responseURL; // window.URL.createObjectURL(new Blob([data.data]));
        const link = document.createElement("a");
        link.href = url;
        /* link.setAttribute(
          "download",
          `certificate_${date.getFullYear()}${date.getMonth()}${date.getDay()}${date.getHours()}${date.getMinutes()}${date.getSeconds()}.pdf`
        ); */ //or any other extension
        link.setAttribute("target", `_blank`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(
            getLocal("Failed To Download") + "! " + error.response.data.title
          );
          showNotification("tr");
        }
      });
  }

  return (
    <ul
      style={{
        listStyle: "none",
      }}
    >
      {laws.map((row, index) => (
        <li key={index} style={{ marginBottom: "7px" }}>
          <a
            className="text-primary"
            onClick={() => downloadDoc(row)}
            title={getLocalByVal(row.brief, row.briefLocal)}
          >
            {getLocalByVal(row.title, row.titleLocal)}
          </a>
        </li>
      ))}
    </ul>
  );
}
