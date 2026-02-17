import React from "react";
import "bootstrap/dist/css/bootstrap.css";
import "./ServiceRequestDetailTbl.css";
import { Col, Row, Table } from "react-bootstrap";

export default function ServiceRequestFormAttachments({
  attachments,
  setAttachments,
  showNotification,
  setErrorMsg,
}) {
  const title = React.useRef("");
  const file = React.useRef("");
  const AddAttachment = () => {
    if (!title.current.value || title.current.value.trim() == "") {
      setErrorMsg("Title is required");
      showNotification("tr");
      return;
    }
    if (attachments.findIndex((a) => a.title == title.current.value) >= 0) {
      setErrorMsg("Title already use for other attachment");
      showNotification("tr");
      return;
    }
    if (!file.current.files || !file.current.files[0]) {
      setErrorMsg("File is required");
      showNotification("tr");
      return;
    }
    const newList = [...attachments];
    newList.push({
      title: title.current.value,
      file: file.current.files[0],
    });
    setAttachments(newList);
    title.current.value = "";
    file.current.value = "";
  };
  const RemoveAttachment = (att) => {
    const newList = attachments.filter((a) => a.title != att.title);
    setAttachments([...newList]);
  };
  return (
    <>
      <>
        <strong>Add More</strong> &nbsp;&nbsp;&nbsp;
        <Row>
          <Col md="6">
            Title:{" "}
            <input
              type="text"
              name={"attachmentTitle"}
              ref={title}
              className="form-control"
            />{" "}
          </Col>
          <Col md="4">
            {" "}
            File:{" "}
            <input
              type="file"
              name={"addAttachment"}
              ref={file}
              className="form-control"
            />
          </Col>
          <Col md="2">
            <button
              type="button"
              class="btn-sm btn btn-outline-success"
              style={{ float: "none" }}
              onClick={(e) => AddAttachment()}
            >
              Add
            </button>
          </Col>{" "}
        </Row>
      </>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>Attachments</th>
            <th>File</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {attachments.length > 0 ? (
            attachments.map((att) => (
              <tr>
                <td>{att.title}</td>
                <td>{att.file ? att.file.name : ""}</td>
                <td>
                  <button
                    type="button"
                    class="btn-sm btn btn-warning "
                    style={{ float: "none", margin: "0" }}
                    onClick={() => RemoveAttachment(att)}
                  >
                    {" "}
                    Remove
                  </button>
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
