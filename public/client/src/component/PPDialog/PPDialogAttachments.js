import { Snackbar } from "@material-ui/core";
import { AddAlert } from "@material-ui/icons";
import React from "react";
import { Table } from "react-bootstrap";
import { Link } from "react-router-dom";
import { downloadPPDialogAttachment } from "../../services/resources";
import "../ServiceRequest/ServiceRequestDetailTbl.css";

export default function PPDialogAttachments({ attachments }) {
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
        console.log(error.response);
        if (error.response) {
          setErrorMsg("Failed To Download");
          showNotification("tr");
        }
      });
  }
  return (
    <>
      {attachments ? (
        <>
          <strong>Attachments</strong>
          <Table striped bordered hover size="sm" className="three-dtbl">
            <thead>
              <tr>
                <th>Title</th>
              </tr>
            </thead>
            <tbody>
              {attachments.length > 0 ? (
                attachments.sort(compare).map((att) => (
                  <tr>
                    <td>
                      <Link onClick={() => downloadDoc(att)}>
                        {att.title}
                      </Link>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colspan="1" className="text-center">
                    <strong>Empty List</strong>
                  </td>
                </tr>
              )}
            </tbody>
          </Table>
        </>
      ) : null}
      <Snackbar
        place="tr"
        color="danger"
        icon={AddAlert}
        message={errorMsg ? errorMsg : "Failed to Download"}
        open={error}
        closeNotification={() => setError(false)}
        close
      />
    </>
  );
}
