import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "./ServiceRequestDetailTbl.css";
import { Col, Row, Table } from "react-bootstrap";
import { deleteRequestAttachment, saveRequestAttachment } from "../../services/serviceRequest";

export default function ServiceRequestAttachments({
  loadRequest,
  serviceType,
  attachments,
  showNotification,
  setErrorMsg,
  requestId,
  showUpdateButtons
}) {
  const getAttachment = (att) => {
    if (
      !att.extension ||
      att.extension.trim() == "" ||
      !serviceType.serviceOwner
    )
      return "-";
    const fileName = att.id.substr(att.id.length - 15).replace("-", "");
    const relativePath = `${serviceType.serviceOwner.toLowerCase()}/documents/${fileName}${
      att.extension
    }`;

    return (
      <a
        href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
        target="_blank"
        rel="noreferrer"
        title={att.title}
        download={att.title}
      >
        {att.title}
      </a>
    );
  };
  const SaveAttachment = (e) => {
    e.preventDefault();
    var formData = new FormData();
    formData.append("Title", e.target[0].value);
    formData.append("attachment", e.target[1].files[0]);
    formData.append("ServiceTypeId", serviceType.id);
    formData.append("RequestId", requestId);
    saveRequestAttachment(formData)
      .then((data) => {
        if (data.data.saved) {
          loadRequest();
          e.target[0].value = "";
          e.target[1].value = "";
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error.response);
        showNotification("tr");
      });
  };
  const RemoveAttachment = (att) => {
    deleteRequestAttachment(att.id)
      .then((data) => {
        if (data.data.success) {
          loadRequest();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error.response);
        showNotification("tr");
      });
  };
  return (
    <>
      {showUpdateButtons ? <><strong>Add More</strong> &nbsp;&nbsp;&nbsp;
      <form onSubmit={(e) => SaveAttachment(e)}>
          <Row>
              <Col md="6">
         
        Title: <input type="text" name={"attachmentTitle"} required className="form-control" />{" "}</Col>
        
        <Col  md="4"> File: <input type="file" name={"addAttachment"} required className="form-control" /></Col>
  
        <Col  md="2">
        <button
          type="submit"
          class="btn-sm btn btn-warning"
          style={{ float: "none" }}
        >
          Save
        </button></Col> </Row>
      </form></> : null }
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>Attachments</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {attachments.length > 0 ? (
            attachments.map((att) => (
              <tr>
                <td>{getAttachment(att)}</td>
                <td>
                {showUpdateButtons ?  <button
                    type="button"
                    class="btn-sm btn btn-warning "
                    style={{ float: "none", margin: "0" }}
                    onClick={() => RemoveAttachment(att)}
                  >
                    {" "}
                    Remove
                  </button> : null }
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={2} className="text-center">
                <strong>No Attachments to show</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
