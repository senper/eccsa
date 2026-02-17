import React, { useState } from "react";
import { Grid, Snackbar } from "@material-ui/core";
import { Row } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import { AddAlert } from "@material-ui/icons";
import "../../App.css";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import { useLocation } from "react-router-dom";
import { useForm } from "react-hook-form";
import { useHistory } from "react-router-dom";
import {
  is_valid_Decimal,
  is_valid_Integer,
} from "../../utils/inputValidations";
import { getLegalProfileLookup } from "../../services/legalProfile";
import { getExportAgreementLookup } from "../../services/exportAgreement";
import {
  getAssociationForApply,
  getMembershipRequest,
  saveMembershipApplication,
} from "../../services/membership";
import ServiceRequestFormAttachments from "../ServiceRequest/ServiceRequestFormAttachments";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function MembershipApplyForm() {
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
  let associationIdValue = 0;
  let requestIdValue = null;
  let typeValue = 0;
  if (location.state) {
    const { associationId, requestId, type } = location.state;
    associationIdValue = associationId;
    requestIdValue = requestId;
    typeValue = type;
  }
  const [association, setAssociation] = React.useState(null);
  const [requirements, setRequirements] = React.useState([]);
  React.useEffect(() => {
    if (associationIdValue) {
      getAssociationForApply(associationIdValue).then((data) => {
        setAssociation(data.data);
        if (
          data.data &&
          data.data.membershipRequirements &&
          data.data.membershipRequirements.$values
        ) {
          const list = data.data.membershipRequirements.$values.filter(
            (r) =>
              (typeValue == 0 && r.applicationRequirementLevelValue != 0) ||
              (typeValue == 2 && r.renewalRequirementLevelValue != 0)
          );
          setRequirements(list);
        }
      });
    }
  }, [associationIdValue]);
  //
  const [request, setRequest] = React.useState([]);
  const [requestDocs, setRequestDocs] = React.useState([]);
  React.useEffect(() => {
    if (requestIdValue) {
      getMembershipRequest(requestIdValue).then((data) => {
        setRequest(data.data);
        setRequestDocs(data.data.responses.$values);
      });
    }
  }, [requestIdValue]);
  //
  function getInput(r) {
    const idStr = r.id.toString();
    const name = idStr.substr(idStr.length - 15).replace("-", "");
    const responseDoc = requestDocs.find((r) => r.requirementId === idStr);
    const readOnly =
      !requestIdValue ||
      request.applicationStatusValue === 0 ||
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
        const relativePath = `membership/documents/${fileName}${responseDoc.textResponse}`;
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
            {association.requirementLookups.$values &&
            association.requirementLookups.$values.find(
              (l) => l.id == r.lookupTypeId
            )
              ? association.requirementLookups.$values
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
      const responseDoc = requestDocs.find((r) => r.requirementId === idStr);
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
    if (requestIdValue) {
      history.push({
        pathname: "/membershipRequestDetail",
        state: {
          applicationId: requestIdValue,
          associationId: associationIdValue,
        },
      });
    } else {
      history.push({
        pathname: "/myServices",
        state: {
          navTo: "membership",
        },
      });
    }
  }
  function onSubmit(inputData) {
    var formData = new FormData();
    formData.append("Id", requestIdValue ? requestIdValue : emptyGuid);
    formData.append("associationId", associationIdValue);
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    attachments.map((att) => {
      formData.append("AttachmentTitle", att.title);
      formData.append(att.title, att.file);
    });
    saveMembershipApplication(formData)
      .then((data) => {
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
      {association ? (
        <div class="row">
          <div class="bg">{association.name}</div>
          <Grid item xs={1} sm={2}></Grid>{" "}
          <Grid item xs={10} sm={6}>
            <form
              onSubmit={handleSubmit(onSubmit)}
              enctype="multipart/form-data"
            >
              {requirements
                ? requirements.sort(compare).map((req) => (
                    <Row xs={10} md={8}>
                      {getInputLabel(req)}
                      {getInput(req)} {setInputValue(req)}
                    </Row>
                  ))
                : null}
              <br />
              {/* {association &&
              association.allowAdditionalAttachments &&
              (!requestIdValue || requestIdValue == emptyGuid) ? (
                <>
                  <ServiceRequestFormAttachments
                    attachments={attachments}
                    setAttachments={setAttachments}
                    showNotification={showNotification}
                    setErrorMsg={setErrorMsg}
                  />
                </>
              ) : null} */}
              <br />
              <button type="submit" class="btn btn-success">
                Save
              </button>
              <button type="button" class="btn btn-warning" onClick={Redirect}>
                Cancel
              </button>
            </form>
            <br />
          </Grid>
          <Grid item xs={1} sm={2}></Grid>
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
