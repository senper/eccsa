import React, { useState } from "react";
import { Grid, Snackbar } from "@material-ui/core";
import { Row } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import { AddAlert } from "@material-ui/icons";
import "../../App.css";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { getServiceType } from "../../services/servicetype";
import { useLocation } from "react-router-dom";
import { useForm } from "react-hook-form";
import { saveRequest, getServiceRequest, excapeCharsReplacer } from "../../services/serviceRequest";
import { useHistory } from "react-router-dom";
import {
  is_validate_email,
  is_validate_mobile,
  is_valid_Decimal,
  is_valid_Integer,
  is_valid_TIN,
  getNameFromId,
} from "../../utils/inputValidations";
import { getLegalProfileLookup } from "../../services/legalProfile";
import { getExportAgreementLookup } from "../../services/exportAgreement";
import ServiceRequestFormAttachments from "./ServiceRequestFormAttachments";
import TabularRequirementForm from "./TabularRequirementForm";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function ServiceRequestForm() {
  const [saving, setSaving] = React.useState(false);
  //
  const [tabularResponses, setTabularResponses] = React.useState([]);
  const [attachments, setAttachments] = React.useState([]);
  //
  const [legalProfiles, setLegalProfiles] = React.useState([]);
  React.useEffect(() => {
    getLegalProfileLookup(1).then((data) => {
      setLegalProfiles(data.data);
    });
  }, []);
  const [exportAgreements, setExportAgreements] = React.useState([]);
  React.useEffect(() => {
    getExportAgreementLookup(1).then((data) => {
      setExportAgreements(data.data);
    });
  }, []);
  //
  const location = useLocation();
  const history = useHistory();
  const {
    register,
    handleSubmit,
    setValue,
    formState, //: { errors },
  } = useForm();
  //
  let serviceTypeIdValue = 0;
  let serviceRequestIdValue = null;
  if (location.state) {
    const { serviceTypeId, serviceRequestId } = location.state;
    serviceTypeIdValue = serviceTypeId;
    serviceRequestIdValue = serviceRequestId;
  }
  const [serviceType, setServiceType] = React.useState([]);
  React.useEffect(() => {
    if (serviceTypeIdValue) {
      getServiceType(serviceTypeIdValue).then((data) => {
        setServiceType(data.data);
      });
    }
  }, [serviceTypeIdValue]);
  //
  const [serviceRequest, setServiceRequest] = React.useState([]);
  const [serviceRequestDocs, setServiceRequestDocs] = React.useState([]);
  React.useEffect(() => {
    if (serviceRequestIdValue) {
      getServiceRequest(serviceRequestIdValue).then((data) => {
        setServiceRequest(data.data);
        setServiceRequestDocs(data.data.responses.$values);
        //
        const responses = data.data.responses.$values.filter(
          (r) => r.requirement.inputMethodValue == 11
        );
        const resultList = [];
        responses.forEach((r) => {
          if (r.tabularResponse && r.tabularResponse.$values) {
            let rowIds = r.tabularResponse.$values.map((t) => t.rowId);
            //get distinct rows
            rowIds = rowIds.filter((value, index, self) => {
              return self.indexOf(value) === index;
            });
            rowIds.forEach((v) => {
              const rowData = r.tabularResponse.$values.filter(
                (t) => t.rowId == v
              );
              let inputDataStr = [];
              rowData.forEach((d) =>
                inputDataStr.push(`"${d.text}": "${excapeCharsReplacer(d.value)}"`)
              );
              resultList.push({
                id: v,
                reqId: r.requirement.id,
                row: JSON.parse(`{${inputDataStr.toString()}}`),
              });
            });
          }
        });
        setTabularResponses(resultList);
        //
      });
    }
  }, [serviceRequestIdValue]);
  //
  function isReqReadOnly(req) {
    const responseDoc = serviceRequestDocs.find(
      (r) => r.requirementId === req.id
    );
    return !(
      !serviceRequestIdValue ||
      serviceRequest.requestStatusValue === 0 ||
      (responseDoc && responseDoc.reviewStatusValue === 3)
    );
  }
  //
  function getInput(r) {
    const idStr = r.id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    const responseDoc = serviceRequestDocs.find(
      (r) => r.requirementId === idStr
    );
    const readOnly =
      !serviceRequestIdValue ||
      serviceRequest.requestStatusValue === 0 ||
      (responseDoc && responseDoc.reviewStatusValue === 3)
        ? ""
        : "readOnly";
    const disabled = readOnly === "" ? "" : "disabled";
    if (r.inputMethodValue === 1) {
      let docLink = <></>;
      if (responseDoc) {
        const fileName = responseDoc.id
          .substr(responseDoc.id.length - 15)
          .replace("-", "");
        const relativePath = `${serviceType.serviceOwnerStr.toLowerCase()}/documents/${fileName}${
          responseDoc.textResponse
        }`;
        docLink = (
          <a
            href={`${process.env.REACT_APP_API_URL}/${relativePath}`}
            target="_blank"
            rel="noreferrer"
          >
            Saved Document
          </a>
        );
      }
      return (
        <>
          {responseDoc && responseDoc.textResponse ? docLink : null}
          {readOnly === "" ? (
            <input
              title={r.descriptionEng}
              type="file"
              name={name}
              class="form-control "
              {...register(name)}
              style={{ borderColor: formState.errors[name] ? "red" : "" }}
            />
          ) : null}
        </>
      );
    } else if (r.inputMethodValue === 2) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Integer(value) })}
          type="number"
          name={name}
          {...{ readOnly }}
          class="form-control "
          title={
            formState.errors[name] ? "Invalid Number Value" : r.descriptionEng
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 3) {
      return (
        <input
          {...register(name, { validate: (value) => is_valid_Decimal(value) })}
          type="number"
          name={name}
          {...{ readOnly }}
          class="form-control "
          title={
            formState.errors[name] ? "Invalid Decimal Number" : r.descriptionEng
          }
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 4) {
      return (
        <input
          {...register(name, { required: true })}
          type="date"
          name={name}
          title={r.descriptionEng}
          {...{ readOnly }}
          maxLength="100"
          class="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 9) {
      //Invoice and Payment
      /* return (
        <input
          {...register(name)}
          type="text"
          name={name}
          {...{ readOnly }}
          maxLength="100"
          class="form-control "
          title={formState.errors[name] ? "Invalid Input" : ""}
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      ); */
    } else if (r.inputMethodValue === 5) {
      return (
        <input
          {...register(name, { required: true })}
          type="text"
          name={name}
          title={r.descriptionEng}
          {...{ readOnly }}
          class="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        />
      );
    } else if (r.inputMethodValue === 6) {
      return (
        <textarea
          {...register(name, { required: true })}
          rows="4"
          name={name}
          title={r.descriptionEng}
          {...{ readOnly }}
          class="form-control "
          style={{ borderColor: formState.errors[name] ? "red" : "" }}
        ></textarea>
      );
    } else if (r.inputMethodValue === 7) {
      //LegalDocument
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            class="form-control "
            title={formState.errors[name] ? "Invalid Input" : r.descriptionEng}
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {legalProfiles.$values
              ? legalProfiles.$values.map((lp) => (
                  <option value={lp.id}>{lp.name}</option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 8) {
      //TradeAgreement
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            class="form-control "
            title={formState.errors[name] ? "Invalid Input" : r.descriptionEng}
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {exportAgreements.$values
              ? exportAgreements.$values.map((lp) => (
                  <option value={lp.id}>{lp.name}</option>
                ))
              : null}
          </select>
        </>
      );
    } else if (r.inputMethodValue === 10) {
      //Lookup
      return (
        <>
          <select
            {...register(name, { required: true })}
            name={name}
            {...{ disabled }}
            class="form-control "
            title={formState.errors[name] ? "Invalid Input" : r.descriptionEng}
            style={{ borderColor: formState.errors[name] ? "red" : "" }}
          >
            {serviceType.requirementLookups.$values &&
            serviceType.requirementLookups.$values.find(
              (l) => l.id == r.lookupTypeId
            )
              ? serviceType.requirementLookups.$values
                  .find((l) => l.id == r.lookupTypeId)
                  .lookupValues.$values.map((lp) => (
                    <option value={lp.id}>{lp.name}</option>
                  ))
              : null}
          </select>
        </>
      );
    }
  }
  function setInputValue(r) {
    if (formState && formState.isDirty) return;
    if (r.inputMethodValue !== 1 && r.id !== emptyGuid) {
      const idStr = r.id.toString();
      const name = idStr.substr(idStr.length - 15).replace("-", "");
      const responseDoc = serviceRequestDocs.find(
        (r) => r.requirementId === idStr
      );
      let value = responseDoc ? responseDoc.response : "";
      if (
        responseDoc &&
        (responseDoc.requirement.inputMethodValue === 7 ||
          responseDoc.requirement.inputMethodValue == 8 ||
          responseDoc.requirement.inputMethodValue == 10)
      ) {
        value = responseDoc.lookupValueId;
      }
      setValue(name, value);
    }
  }
  function Redirect() {
    if (serviceRequestIdValue) {
      history.push({
        pathname: "/serviceRequestDetail",
        state: {
          serviceRequestId: serviceRequestIdValue,
          serviceTypeId: serviceTypeIdValue,
        },
      });
    } else {
      history.push({
        pathname: "/myServices",
        state: {
          navTo: "request",
        },
      });
    }
  }
  function onSubmit(inputData) {
    setSaving(true);
    var formData = new FormData();
    formData.append(
      "Id",
      serviceRequestIdValue ? serviceRequestIdValue : emptyGuid
    );
    formData.append("ServiceTypeId", serviceTypeIdValue);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    attachments.forEach((att) => {
      formData.append("AttachmentTitle", att.title);
      formData.append(att.title, att.file);
    });
    serviceType.serviceRequirements.$values
      .filter((r) => r.inputMethodValue == 11)
      .forEach((tr) => {
        const formattedData = [];
        tabularResponses
          .filter((t) => t.reqId == tr.id)
          .forEach((v) => {
            for (const [key, value] of Object.entries(v.row)) {
              formattedData.push({ rowId: v.id, text: key, value: value });
            }
          });
        formData.append(getNameFromId(tr.id), JSON.stringify(formattedData));
      });
    saveRequest(formData)
      .then((data) => {
        setSaving(false);
        if (data.data.saved) {
          Redirect();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        setSaving(false);
        console.log(error);
        showNotification("tr");
      });
  }
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 5000);
        }
        break;
      default:
        break;
    }
  };
  function compare(a, b) {
    if (a.orderNo < b.orderNo) {
      return -1;
    }
    if (a.orderNo > b.orderNo) {
      return 1;
    }
    return 0;
  }
  const getInputLabel = (req) => {
    if (!req || req.inputMethodValue === 9 || req.requirementTypeValue == 2)
      return;
    var value = req.label.trim();
    if (req.requirementType === "Header") {
      return (
        <label>
          <h3>{value}</h3>
        </label>
      );
    } else {
      return <label>{value}</label>;
    }
  };
  return (
    <div>
      {/* <Topbar/> */}
      {serviceType ? (
        <div class="row">
          <div class="bg">{serviceType.name}</div>
          <Grid item sm={1} md={2}></Grid>{" "}
          <Grid item sm={11} md={10}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              enctype="multipart/form-data"
            >
              {serviceType.serviceRequirements &&
              serviceType.serviceRequirements.$values ? (
                <>
                  <Grid item sm={1} md={2}></Grid>{" "}
                  <Grid item sm={10} md={6}>
                    {serviceType.serviceRequirements.$values
                      .filter((r) => r.inputMethodValue != 11)
                      .sort(compare)
                      .map((req) => (
                        <Row xs={10} md={8}>
                          {getInputLabel(req)}
                          {getInput(req)} {setInputValue(req)}
                          {/* {errors[req.id]} */}
                        </Row>
                      ))}
                  </Grid>
                  <br />
                  <Grid item sm={11} md={10}>
                    {serviceType.serviceRequirements.$values
                      .filter((r) => r.inputMethodValue == 11)
                      .sort(compare)
                      .map((req) => (
                        <TabularRequirementForm
                          requirement={req}
                          tabularSource={serviceType.tabularRequirements.$values.find(
                            (l) => l.id == req.lookupTypeId
                          )}
                          tabularResponses={tabularResponses.filter(
                            (s) => s.reqId == req.id
                          )}
                          setTabularResponses={setTabularResponses}
                          isReadOnly={isReqReadOnly(req)}
                        />
                      ))}
                  </Grid>
                </>
              ) : null}
              {serviceType &&
              serviceType.allowAdditionalAttachments &&
              (!serviceRequestIdValue || serviceRequestIdValue == emptyGuid) ? (
                <>
              <br />
                  <Grid item sm={1} md={2}></Grid>{" "}
                  <Grid item sm={12} md={10}>
                    <ServiceRequestFormAttachments
                      attachments={attachments}
                      setAttachments={setAttachments}
                      showNotification={showNotification}
                      setErrorMsg={setErrorMsg}
                    />
                  </Grid>
                </>
              ) : null}
              <Grid item sm={1} md={2}></Grid>{" "}
              <Grid item sm={12} md={6}>
                <button type="submit" class="btn btn-success" style={{marginTop: "0"}}
                disabled={saving} >
                  {saving ? "Saving" : "Save"}
                </button>
                <button
                  type="button"
                  class="btn btn-warning"
                  onClick={Redirect}
                  style={{marginTop: "0"}}
                >
                  Cancel
                </button>
              </Grid>
            </form>
            <br />
          </Grid>
          <Grid item sm={1} md={2}></Grid>
        </div>
      ) : null}
      <Grid container spacing={2}>
        <br />
        <br />
  
        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg || !error ? errorMsg : "Failed to Save request"}
          open={error}
          closeNotification={() => setError(false)}
          close
        />
      </Grid>
    </div>
  );
}

// export default App;
