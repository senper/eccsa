import React from "react";
import { Col, Row, Table } from "react-bootstrap";

export default function ServiceRequestFormOptionals({
  optionalEntries,
  optionalEntryLookupValues,
  setOptionalEntries,
  showNotification,
  setErrorMsg,
  getLocal,
  getLocalByVal,
}) {
  const label = React.useRef("");
  const value = React.useRef("");
  const AddOptionalEntry = () => {
    if (!label.current.value || label.current.value.trim() == "") {
      setErrorMsg("Label is required");
      showNotification("tr");
      return;
    }
    if (!value.current.value || value.current.value.trim() == "") {
      setErrorMsg("Value is required");
      showNotification("tr");
      return;
    }

    const oldOptionalEntry = optionalEntries.find(
      (a) => a.label == label.current.value
    );
    let newList = [...optionalEntries];
    if (oldOptionalEntry && oldOptionalEntry.label) {
      newList = newList.filter((s) => s.label != label.current.value);
    }
    newList.push({
      label: label.current.value,
      value: value.current.value,
    });
    setOptionalEntries(newList);
    label.current.value = "";
    value.current.value = "";
  };
  const RemoveOptionalEntry = (att) => {
    const newList = optionalEntries.filter((a) => a.label != att.label);
    setOptionalEntries([...newList]);
  };
  return (
    <>
      <>
        <strong>{getLocal("Add Optional Requirements")}</strong>{" "}
        &nbsp;&nbsp;&nbsp;
        <Row>
          <Col md="6">
            {getLocal("Label")}:{" "}
            <select
              name={"optionalEntryLabel"}
              ref={label}
              className="form-control"
            >
              {optionalEntryLookupValues
                ? optionalEntryLookupValues.map((lp, index) => (
                    <option key={index} value={lp.value}>
                      {lp.name}
                    </option>
                  ))
                : null}
            </select>
          </Col>
          <Col md="4">
            {" "}
            {getLocal("Value")}:{" "}
            <input
              type="text"
              name={"optionalEntryValue"}
              ref={value}
              className="form-control"
            />
          </Col>
          <Col md="2">
            <button
              type="button"
              className="btn-sm btn btn-outline-success"
              style={{ float: "none" }}
              onClick={(e) => AddOptionalEntry()}
            >
              {getLocal("Add")}
            </button>
          </Col>{" "}
        </Row>
      </>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            <th>{getLocal("Label")}</th>
            <th>{getLocal("Value")}</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          {optionalEntries.length > 0 ? (
            optionalEntries.map((att, index) => (
              <tr key={index}>
                <td>{att.label}</td>
                <td>{att.value}</td>
                <td>
                  <button
                    type="button"
                    className="btn-sm btn btn-warning "
                    style={{ float: "none", margin: "0" }}
                    onClick={() => RemoveOptionalEntry(att)}
                  >
                    {" "}
                    {getLocal("Remove")}
                  </button>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={2} className="text-center">
                <strong>{getLocal("No Optional Entries to show")}</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
