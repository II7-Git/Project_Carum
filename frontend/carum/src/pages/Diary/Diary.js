import styles from "./Diary.module.css";
import TopNav from "../../components/TopNav";
import Button from "../../components/Button";
import { useParams } from "react-router-dom";
import { useEffect, useState, useRef } from "react";
import { fetchDiary } from "apis/diary";
import { Viewer } from "@toast-ui/react-editor";
import moment from "moment";
import sadImg from "assets/sad.svg";
import angryImg from "assets/angry.svg";
import worryImg from "assets/worry.svg";
import happyImg from "assets/happy.svg";
import surpriseImg from "assets/surprise.svg";
import peaceImg from "assets/peace.svg";
import DiaryWrite from "./DiaryWrite";
import { deleteDiaryContent, editDiary } from "apis/diary";
import Swal from "sweetalert2";

const WEEK_DAY = [
  "일요일",
  "월요일",
  "화요일",
  "수요일",
  "목요일",
  "금요일",
  "토요일",
];

const setBackgroundColor = (color) => {
  if (color === "red") {
    return styles.red;
  } else if (color === "orange") {
    return styles.orange;
  } else if (color === "yellow") {
    return styles.yellow;
  } else if (color === "green") {
    return styles.green;
  } else if (color === "blue") {
    return styles.blue;
  } else if (color === "indigo") {
    return styles.indigo;
  } else if (color === "purple") {
    return styles.purple;
  }
};

const color = ["red", "orange", "yellow", "green", "blue", "indigo", "purple"];

function Diary({ unityRef }) {
  const [diary, setDiary] = useState();
  const [curState, setCurState] = useState("read");
  const [colorBar, setColorBar] = useState(color);
  const [showColorPickerBar, setShowColorPickerBar] = useState(false);
  const [curBackgroundColor, setCurBackgroundColor] = useState("indigo");
  const { id } = useParams();

  // unity

  const enterCloseUp = () => {
    unityRef.current.enterCloseUp();
  };
  const exitCloseUp = () => {
    unityRef.current.exitCloseUp();
  };

  const fetchDiarySuccess = (res) => {
    console.log(res);
    setDiary(res.data);

    const tmpColors = [...color];

    tmpColors.splice(color.indexOf(res.data.background), 1);
    setColorBar(tmpColors);
    setCurBackgroundColor(res.data.background);
  };

  const fetchDiaryFail = (err) => {
    console.log(err);
  };

  useEffect(() => {
    if (curState === "read") {
      fetchDiary(id, fetchDiarySuccess, fetchDiaryFail);
    }
  }, [curState]);

  useEffect(() => {
    return () => {
      if (diary?.background !== curBackgroundColor) {
        const payload = {
          content: diary?.content,
          emotionTag: diary?.emotionTag,
          background: curBackgroundColor,
          diaryId: id,
        };

        console.log("색깔 다르다");

        editDiary(
          payload,
          (res) => {
            console.log(res);
          },
          (err) => {
            console.log(err);
          }
        );
      }
    };
  }, []);

  // 다이어리 비우기
  const deleteDiaryContentSuccess = (res) => {
    console.log(res);
    setDiary(null);
    fetchDiary(id, fetchDiarySuccess, fetchDiaryFail);
  };

  const deleteDiaryContentFail = (err) => {
    console.log(err);
  };

  const handleDeleteContent = () => {
    Swal.fire({
      title: "정말 내용을 비우시겠습니까?",
      showDenyButton: true,
      showConfirmButton: false,
      showCancelButton: true,
      denyButtonText: "비우기",
      cancelButtonText: "취소",
    }).then((result) => {
      if (result.isDenied) {
        deleteDiaryContent(
          id,
          deleteDiaryContentSuccess,
          deleteDiaryContentFail
        );
      }
    });
  };

  return (
    <div>
      {curState === "read" ? (
        <div>
          <TopNav text="내 일기" />
          <div className={styles.contentContainer}>
            <div className={styles.emotionBox}>
              {diary?.emotionTag.map((e) => {
                return (
                  <img
                    className={styles.emotionImg}
                    src={
                      e === "SAD"
                        ? sadImg
                        : e === "ANGRY"
                        ? angryImg
                        : e === "PEACE"
                        ? peaceImg
                        : e === "HAPPY"
                        ? happyImg
                        : e === "SURPRISE"
                        ? surpriseImg
                        : e === "WORRY"
                        ? worryImg
                        : null
                    }
                    alt="emotion"
                    key={e}
                  />
                );
              })}
            </div>
            <div
              className={`${styles.contentBox} ${setBackgroundColor(
                curBackgroundColor
              )}`}
            >
              <p>{moment(diary?.createDate).format("YYYY-MM-DD")}</p>
              <p>{WEEK_DAY[new Date(diary?.createDate).getDay()]}</p>
              <div
                className={`${styles.colorPicker} ${setBackgroundColor(
                  curBackgroundColor
                )}`}
                onClick={() => setShowColorPickerBar(!showColorPickerBar)}
              ></div>
              {showColorPickerBar ? (
                <div className={styles.colorPickerBar}>
                  {colorBar.map((col, idx) => (
                    <div
                      className={`${styles.colors} ${setBackgroundColor(col)}`}
                      key={idx}
                      onClick={() => {
                        setCurBackgroundColor(col);
                        setShowColorPickerBar(false);
                      }}
                    ></div>
                  ))}
                </div>
              ) : null}
              <div className={styles.content}>
                {diary ? <Viewer initialValue={diary.content} /> : null}
              </div>
            </div>
            {moment(diary?.createDate).format("YYYY-MM-DD") ===
            moment(new Date()).format("YYYY-MM-DD") ? (
              <div className={styles.buttonBox}>
                <Button
                  text="일기 수정"
                  variant="light"
                  size="small"
                  onClick={() => setCurState("edit")}
                />
                <Button
                  text="일기 비우기"
                  variant="light"
                  size="small"
                  onClick={handleDeleteContent}
                />
              </div>
            ) : (
              <Button
                text="일기 비우기"
                variant="light"
                size="big"
                onClick={handleDeleteContent}
              />
            )}
          </div>
        </div>
      ) : curState === "edit" ? (
        <DiaryWrite
          setDiary={setDiary}
          setCurState={setCurState}
          state="edit"
          diary={diary}
          diaryId={id}
          enterCloseUp={enterCloseUp}
          exitCloseUp={exitCloseUp}
        />
      ) : null}
    </div>
  );
}

export default Diary;
