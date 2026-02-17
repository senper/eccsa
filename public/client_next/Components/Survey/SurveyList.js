import React from "react";
import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
} from "@mui/material";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPoll } from "@fortawesome/free-solid-svg-icons";
import Link from "next/link";
import CustomDataGrid from "../MaterialUIDataGrid/CustomDataGrid";
import { renderCellExpand } from "../MaterialUIDataGrid/renderCellExpand";
import { getSurveyList } from "../../api/survey";
import { Card, Col, Row } from "react-bootstrap";

export default function SurveyList({
  expanded = true,
  getLocal,
  getLocalByVal,
}) {
  const token = localStorage.getItem("access_token");
  const [surveys, setSurveys] = React.useState([]);
  React.useEffect(() => {
    if (token) {
      getSurveyList().then((data) => {
        setSurveys(data.data.$values);
      });
    }
  }, [token]);

  const columns = [
    {
      headerName: getLocal("Title"),
      field: "title",
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Description"),
      field: "description",
      flex: 0.25,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Open Days"),
      field: "setting.openDays",
      flex: 0.1,
      valueGetter: (params) => {
        return params.row.setting.openDays;
      },
    },
    {
      headerName: getLocal("Status"),
      field: "setting",
      valueGetter: (params) => {
        const questionsCount = params.value.questionsCount;
        const responsesCount = params.value.responsesCount;
        return `${responsesCount} of ${questionsCount}`;
      },
      flex: 0.2,
      renderCell: renderCellExpand,
    },
    {
      headerName: getLocal("Actions"),
      field: "id",
      flex: 0.09,
      filterable: false,
      sortable: false,
      // eslint-disable-next-line react/display-name
      renderCell: (params) => (
        <div className="actions-right">
          <Link
            href={`/myServices/surveyQuestionForm?surveyId=${params.value}`}
            legacyBehavior>
            {getLocal("Detail")}
          </Link>
        </div>
      ),
    },
  ];
  return <>
    <Grid item xs={12}>
      <Accordion defaultExpanded={expanded}>
        <AccordionSummary expandIcon={<ExpandMoreIcon></ExpandMoreIcon>}>
          <div className="icon">
            <FontAwesomeIcon icon={faPoll} size="1x" />
          </div>
          <div className="text-xs font-weight-bold text-primary text-uppercase mb-1">
            <p className="links"> {getLocal("Surveys")}</p>
          </div>
        </AccordionSummary>
        <AccordionDetails>
          <>
            <div id="showOnLargeScreen">
              {<CustomDataGrid dataRows={surveys} columns={columns} />}
            </div>
            <div id="showOnSmallScreen">
              {surveys && surveys.length > 0 ? (
                surveys.map((r, i) => (
                  <Card key={i}>
                    <Card.Header>{r.title}</Card.Header>
                    <Card.Body>
                      <Row>
                        <Col>{getLocal("Description")}</Col>
                        <Col>{r.description}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Open Days")}</Col>
                        <Col>{r.setting.openDays}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Status")}</Col>
                        <Col>{`${r.setting.responsesCount} of ${r.setting.questionsCount}`}</Col>
                      </Row>
                      <Row>
                        <Col>{getLocal("Actions")}</Col>
                        <Col>
                          <Link href={`/myServices/surveyQuestionForm?surveyId=${r.id}`} legacyBehavior>
                            {getLocal("Detail")}
                          </Link>
                        </Col>
                      </Row>
                    </Card.Body>
                  </Card>
                ))
              ) : (
                <>No Survey(s) Found</>
              )}
            </div>
          </>
        </AccordionDetails>
      </Accordion>
    </Grid>
  </>;
}
