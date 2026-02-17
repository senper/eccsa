import React from "react";
import AddAlert from "@mui/icons-material/AddAlert";
import { Snackbar } from "@mui/material";
import { Card, Table } from "react-bootstrap";
import { downloadPPDialogAttachment } from "../../api/resources";

export default function PPDialogAttachments({
  attachments,
  getLocalByVal,
  getLocal,
}) {
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(false);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg("");
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  function compare(a, b) {
    if (a.title < b.title) {
      return -1;
    }
    if (a.title > b.title) {
      return 1;
    }
    return 0;
  }
  function downloadDoc(attachment) {
    downloadPPDialogAttachment(attachment.id)
      .then((data) => {
        const url = data.request.responseURL;
        const link = document.createElement("a");
        link.href = url;
        link.setAttribute("target", `_blank`);
        document.body.appendChild(link);
        link.click();
        link.remove();
      })
      .catch((error) => {
        // console.log(error.response);
        if (error.response) {
          setErrorMsg(getLocal("Failed To Download"));
          showNotification("tr");
        }
      });
  }
  return (
    <>
      {attachments ? (
        <>
          <strong>{getLocal("Attachments")}</strong>
          <div id="showOnLargeScreen">
            <Table striped bordered hover size="sm" className="three-dtbl">
              <thead>
                <tr>
                  <th>{getLocal("Title")}</th>
                </tr>
              </thead>
              <tbody>
                {attachments.length > 0 ? (
                  attachments.sort(compare).map((att, index) => (
                    att ? <tr key={index}>
                      <td>
                        <a
                          onClick={() => downloadDoc(att)}
                          className="text-primary"
                        >
                          {att.title}
                        </a>
                      </td>
                    </tr> : null
                  ))
                ) : (
                  <tr>
                    <td colSpan="1" className="text-center">
                      <strong>{getLocal("Empty List")}</strong>
                    </td>
                  </tr>
                )}
              </tbody>
            </Table>
          </div>
          <div id="showOnSmallScreen">
            {attachments && attachments.length > 0 ? (
              attachments.sort(compare).map((att, i) => (
                <Card key={i} className="card__margin-bottom">
                  <Card.Header>
                  {att ? <a
                      onClick={() => downloadDoc(att)}
                      className="text-primary"
                    >
                      {att.title}
                    </a> : null}
                  </Card.Header>
                  <Card.Body></Card.Body>
                </Card>
              ))
            ) : (
              <Card className="card__margin-bottom">
                <strong>{getLocal("Empty List")}</strong>
              </Card>
            )}
          </div>
        </>
      ) : null}
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : getLocal("Failed to Download")}
        open={error}
        onClose={() => setError(false)}
      />
    </>
  );
}
