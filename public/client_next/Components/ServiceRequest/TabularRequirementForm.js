import React from "react";
import { Col, Form, Row, Table } from "react-bootstrap";
import { useForm } from "react-hook-form";

export default function TabularRequirementForm({
  requirement,
  tabularSource,
  tabularResponses,
  setTabularResponses,
  isReadOnly,
  getLocal,
  getLocalByVal,
  lookups,
}) {
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState, //: { errors },
  } = useForm();
  const [lastId, setLastId] = React.useState(tabularResponses.length);
  React.useEffect(() => {
    let maxId = 0;
    tabularResponses.map((v) => {
      if (maxId < v.id) {
        maxId = v.id;
      }
    });
    setLastId(maxId);
  }, [tabularResponses]);
  function addTabularDataToList(evt) {
    const inputData = getValues();
    const newId = lastId + 1;
    setLastId(newId);
    tabularResponses.push({ id: newId, reqId: requirement.id, row: inputData });
    setTabularResponses([...tabularResponses]);
    for (const [key, value] of Object.entries(inputData)) {
      setValue(key, "");
    }
  }
  function RemoveRow(id) {
    const newList = tabularResponses.filter((t) => t.id != id);
    setTabularResponses([...newList]);
  }
  function compare(a, b) {
    if (a.no < b.no) {
      return -1;
    }
    if (a.no > b.no) {
      return 1;
    }
    return 0;
  }
  function compare2(a, b) {
    if (a.name < b.name) {
      return -1;
    }
    if (a.name > b.name) {
      return 1;
    }
    return 0;
  }
  return (
    <>
      <>
        <strong>
          {getLocalByVal(requirement.label, requirement.labelLocal)}
        </strong>{" "}
        &nbsp;&nbsp;&nbsp;
        {isReadOnly ? null : (
          <>
            <Row>
              {tabularSource.details && tabularSource.details.$values
                ? tabularSource.details.$values
                    .sort(compare)
                    .map((d, index) => (
                      <Col md={6} key={index}>
                        <Form.Label>{getLocal(d.caption)}</Form.Label>
                        {d.dataTypeValue != 3 ? (
                          <input
                            type={d.dataTypeValue == 1 ? "text" : "number"}
                            step={"any"}
                            name={d.id}
                            max={d.maxLength}
                            maxLength={d.maxLength}
                            {...register(d.id)}
                            className="form-control "
                          />
                        ) : (
                          <select
                            name={d.id}
                            {...register(d.id)}
                            className="form-control"
                          >
                            {lookups && lookups.find((l) => l.id == d.lookupId)
                              ? lookups
                                  .find((l) => l.id == d.lookupId)
                                  .lookupValues.$values.sort(compare2).map((lp, index) => (
                                    <option key={index} value={lp.value}>
                                      {lp.name}
                                    </option>
                                  ))
                              : null}
                          </select>
                        )}
                      </Col>
                    ))
                : null}
            </Row>
            <button
              type="button"
              className="btn-sm btn btn-outline-success"
              style={{ float: "none", marginLeft: "0" }}
              onClick={(evt) => addTabularDataToList(evt)}
            >
              {getLocal("Add")}
            </button>
          </>
        )}
      </>
      <Table striped bordered hover size="sm" className="three-dtbl">
        <thead>
          <tr>
            {tabularSource.details && tabularSource.details.$values
              ? tabularSource.details.$values
                  .sort(compare)
                  .map((d, index) => <th key={index}>{getLocal(d.caption)}</th>)
              : null}
            <th style={{ width: "10px" }}></th>
          </tr>
        </thead>
        <tbody>
          {tabularResponses.length > 0 ? (
            tabularResponses.map((r, rindex) => (
              <tr key={rindex}>
                {tabularSource.details && tabularSource.details.$values
                  ? tabularSource.details.$values
                      .sort(compare)
                      .map((d, index) => <td key={index}>{r.row[d.id]}</td>)
                  : null}
                <td>
                  {isReadOnly ? null : (
                    <button
                      type="button"
                      className="btn-sm btn btn-warning "
                      style={{
                        float: "none",
                        margin: "0",
                        backgroundColor: "orange",
                      }}
                      onClick={() => RemoveRow(r.id)}
                    >
                      {" "}
                      {getLocal("Remove")}
                    </button>
                  )}
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td
                colSpan={
                  tabularSource.details && tabularSource.details.$values
                    ? tabularSource.details.$values.length + 1
                    : 1
                }
                className="text-center"
              >
                <strong>{getLocal("No Data to show")}</strong>
              </td>
            </tr>
          )}
        </tbody>
      </Table>
    </>
  );
}
